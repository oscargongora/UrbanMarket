using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Aggregates.Cart;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Shared;
using MediatR;

namespace ChicStreetwear.Application.Carts.Commands;
public sealed record DeleteCartProductResponse(Guid cartId);
public sealed record DeleteCartProductCommand(Guid cartId, Guid cartProductId, string? nameIdentifier = null) : IRequest<Result<DeleteCartProductResponse>>;

public sealed class DeleteCartProductCommandHandler : IRequestHandler<DeleteCartProductCommand, Result<DeleteCartProductResponse>>
{
    private readonly IRepository<Cart> _cartRepository;

    public DeleteCartProductCommandHandler(IRepository<Cart> cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<Result<DeleteCartProductResponse>> Handle(DeleteCartProductCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByIdAsync(request.cartId, cancellationToken);
        if (cart is null)
        {
            return CartErrors.CartNotFound;
        }

        var result = cart.RemoveProduct(request.cartProductId);
        if (result.HasErrors)
        {
            return result.Errors;
        }

        await _cartRepository.UpdateAsync(cart);

        return Result<DeleteCartProductResponse>.Succeeded(new DeleteCartProductResponse(cart.Id));
    }
}