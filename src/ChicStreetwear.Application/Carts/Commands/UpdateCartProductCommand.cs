using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Aggregates.Cart;
using ChicStreetwear.Domain.Aggregates.Product;
using ChicStreetwear.Domain.Aggregates.Product.Entities;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Shared;
using MediatR;

namespace ChicStreetwear.Application.Carts.Commands;
public sealed record UpdateCartProductResult(Guid cartId, Guid cartProductId);

public sealed record UpdateCartProductCommand(Guid cartProductId, Guid productId, Guid? variationId, int quantity, Guid cartId, string? nameIdentifier = null) : IRequest<Result<UpdateCartProductResult>>;

public sealed class UpdateCartProductCommandHandler : IRequestHandler<UpdateCartProductCommand, Result<UpdateCartProductResult>>
{
    private readonly IRepository<Cart> _cartRepository;
    private readonly IRepository<Product> _productRepository;

    public UpdateCartProductCommandHandler(IRepository<Cart> cartRepository, IRepository<Product> productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    public async Task<Result<UpdateCartProductResult>> Handle(UpdateCartProductCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByIdAsync(request.cartId, cancellationToken);
        if (cart is null)
        {
            return CartErrors.CartNotFound;
        }

        var product = await _productRepository.GetByIdAsync(request.productId, cancellationToken);
        if (product is null)
        {
            return ProductErrors.ProductNotFound;
        }

        var maxQuantity = product.Stock.Quantity;

        Variation? variation = default;

        if (product.HasAttributes)
        {
            if (request.variationId is null)
            {
                return Error.BadRequest("UpdateCartProduct", "Variation id is required.");
            }
            variation = product.Variations.FirstOrDefault(v => v.Id.Equals(request.variationId));
            if (variation is null) return ProductErrors.VariationNotFound;
            maxQuantity = variation.Stock.Quantity;
        }

        if (request.quantity > maxQuantity)
        {
            return CartErrors.RequestedQuantityExceedsQuantityInStock;
        }

        var result = cart.UpdateProduct(request.cartProductId, request.quantity);
        if (result.HasErrors)
        {
            return result.Errors;
        }

        await _cartRepository.UpdateAsync(cart);

        return Result<UpdateCartProductResult>.Succeeded(new UpdateCartProductResult(cart.Id, request.cartProductId));
    }
}
