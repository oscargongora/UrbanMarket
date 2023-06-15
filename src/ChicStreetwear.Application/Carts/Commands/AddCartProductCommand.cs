using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Aggregates.Cart;
using ChicStreetwear.Domain.Aggregates.Cart.Entities;
using ChicStreetwear.Domain.Aggregates.Cart.ValueObjects;
using ChicStreetwear.Domain.Aggregates.Product;
using ChicStreetwear.Domain.Aggregates.Product.Entities;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Shared;
using MediatR;

namespace ChicStreetwear.Application.Carts.Commands;
public sealed record AddCartProductResult(Guid cartId, Guid cartProductId);

public sealed record AddCartProductCommand(Guid productId, Guid? variationId, int quantity, Guid? cartId = null, string? nameIdentifier = null) : IRequest<Result<AddCartProductResult>>;

public sealed class AddCartProductCommandHandler : IRequestHandler<AddCartProductCommand, Result<AddCartProductResult>>
{
    private readonly IRepository<Cart> _cartRepository;
    private readonly IRepository<Product> _productRepository;

    public AddCartProductCommandHandler(IRepository<Cart> cartRepository, IRepository<Product> productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    public async Task<Result<AddCartProductResult>> Handle(AddCartProductCommand request, CancellationToken cancellationToken)
    {
        bool isNewCart = false;
        Cart? cart;
        if (request.cartId is null)
        {
            cart = Cart.New(request.nameIdentifier is null ? null : Guid.Parse(request.nameIdentifier));
            isNewCart = true;
        }
        else
        {
            cart = await _cartRepository.FirstOrDefaultAsync(c => c.Id.Equals(request.cartId), false, cancellationToken);

            if (cart is null)
                return CartErrors.CartNotFound;
        }

        var product = await _productRepository.GetByIdAsync(request.productId, cancellationToken);
        if (product is null)
        {
            return ProductErrors.ProductNotFound;
        }

        var maxQuantity = product.Stock.Quantity;
        var price = product.SalePrice?.Amount ?? product.RegularPrice?.Amount;

        Variation? variation = default;

        if (product.HasAttributes)
        {
            if (request.variationId is null)
            {
                return Error.BadRequest("AddCartProduct", "Variation id is required.");
            }
            variation = product.Variations.FirstOrDefault(v => v.Id.Equals(request.variationId));
            if (variation is null) return ProductErrors.VariationNotFound;
            maxQuantity = variation.Stock.Quantity;
            price = variation.SalePrice?.Amount ?? variation.RegularPrice.Amount;
        }

        if (request.quantity > maxQuantity)
        {
            return CartErrors.RequestedQuantityExceedsQuantityInStock;
        }

        if (price is null)
        {
            return Error.BadRequest("AddCartProduct", "There was a problem calculating the price");
        }

        var cartProduct = CartProduct.New(request.productId, product.Name, product.Description, request.quantity, (decimal)price, product.SellerId, variation?.Attributes?.ToList().ConvertAll(a => CartProductAttribute.New(a.Attribute.Name, a.Value)), variation?.Id);

        var result = cart.AddProduct(cartProduct);
        if (result.HasErrors)
        {
            return result.Errors;
        }

        if (isNewCart)
            await _cartRepository.AddAsync(cart);
        else
            await _cartRepository.UpdateAsync(cart);

        return Result<AddCartProductResult>.Succeeded(new AddCartProductResult(cart.Id, cartProduct.Id));
    }
}
