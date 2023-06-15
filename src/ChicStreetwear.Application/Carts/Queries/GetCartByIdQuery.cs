using ChicStreetwear.Application.Carts;
using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Aggregates.Cart;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Shared;
using MediatR;

namespace ChicStreetwear.Application.Categories.Queries;
public sealed record GetCartByIdQuery(Guid Id) : IRequest<Result<CartResponse>>;

public sealed class GetCartByIdQueryHandler : IRequestHandler<GetCartByIdQuery, Result<CartResponse>>
{
    private readonly IRepository<Cart> _cartRepository;

    public GetCartByIdQueryHandler(IRepository<Cart> cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<Result<CartResponse>> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByIdAsync(request.Id);
        if (cart is null)
        {
            return CartErrors.CartNotFound;
        }
        return Result<CartResponse>.Succeeded(CartResponse.FromCart(cart));
    }
}
