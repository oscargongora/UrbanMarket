using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Application.Common.Queries;
using ChicStreetwear.Domain.Aggregates.Cart;
using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Models.Components;
using MediatR;

namespace ChicStreetwear.Application.Carts.Queries;
public sealed class GetCartsQuery : GetQueryBase, IRequest<Result<PaginatedListModel<CartResponse>>> { }

public sealed class GetCartsQueryHandler : IRequestHandler<GetCartsQuery, Result<PaginatedListModel<CartResponse>>>
{
    private readonly IRepository<Cart> _cartRepository;

    public GetCartsQueryHandler(IRepository<Cart> cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<Result<PaginatedListModel<CartResponse>>> Handle(GetCartsQuery request, CancellationToken cancellationToken)
    {
        var carts = await _cartRepository.PaginatedListAsync(predicates: new(), includes: new(), request.Page ?? 1, request.Take ?? 10, true, cancellationToken);

        PaginatedListModel<CartResponse> response = new(carts.Items.ConvertAll(c => CartResponse.FromCart(c)), carts.TotalItems);

        return Result<PaginatedListModel<CartResponse>>.Succeeded(response);
    }
}