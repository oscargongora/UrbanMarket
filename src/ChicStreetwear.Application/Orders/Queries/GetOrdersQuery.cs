using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Application.Common.Mappers;
using ChicStreetwear.Application.Common.Queries;
using ChicStreetwear.Domain.Aggregates.Order;
using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Models.Components;
using ChicStreetwear.Shared.Models.Order;
using MediatR;
using System.Linq.Expressions;

namespace ChicStreetwear.Application.Orders.Queries;
public sealed class GetOrdersQuery : GetQueryBase, IRequest<Result<PaginatedListModel<OrderModel>>>
{
    public Guid? CustomerId { get; set; }
}

public sealed class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, Result<PaginatedListModel<OrderModel>>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<PaginatedListModel<OrderModel>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        List<Expression<Func<Order, bool>>> predicates = new();
        if (request.CustomerId is not null)
        {
            predicates.Add(o => o.CustomerId.Equals(request.CustomerId));
        }
        var orders = await _orderRepository.PaginatedListAsync(predicates,
                                              new(),
                                              request.Page ?? 1,
                                              request.Take ?? 10,
                                              true,
                                              cancellationToken);

        return Result<PaginatedListModel<OrderModel>>.Succeeded(new(orders.Items.ConvertAll(o => o.ToOrderModel()), orders.TotalItems));
    }
}

