using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Application.Common.Mappers;
using ChicStreetwear.Application.Common.Queries;
using ChicStreetwear.Domain.Aggregates.Order;
using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Models.Components;
using ChicStreetwear.Shared.Models.Order;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace ChicStreetwear.Application.Orders.Queries;

public sealed class GetOrdersDataGridQuery : GetQueryBase, IRequest<Result<PaginatedListModel<OrderModel>>>
{
    public bool IsAdministrator { get; set; } = false;
    public Guid UserId { get; set; }
}

public sealed class GetOrdersDataGridQueryHandler : IRequestHandler<GetOrdersDataGridQuery, Result<PaginatedListModel<OrderModel>>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<GetOrdersDataGridQueryHandler> _logger;

    public GetOrdersDataGridQueryHandler(IOrderRepository orderRepository, ILogger<GetOrdersDataGridQueryHandler> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<Result<PaginatedListModel<OrderModel>>> Handle(GetOrdersDataGridQuery request, CancellationToken cancellationToken)
    {
        List<Expression<Func<Order, bool>>> predicates = new();
        List<Expression<Func<Order, object>>> includes = new();

        if (!request.IsAdministrator)
        {
            includes.Add(o => o.Products.Where(p => p.SellerId.Equals(request.UserId)));
            predicates.Add(o => o.Products.Any(p => p.SellerId.Equals(request.UserId)));
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            if (Guid.TryParse(request.Search, out Guid searchGuid))
            {
                predicates.Add(o => o.Id.Equals(searchGuid));
            }
        }

        var ordersDataGrid = await _orderRepository.PaginatedListAsync(
            predicates,
            includes,
            request.Sorts,
            request.Filters,
            request.Page ?? 1,
            request.Take ?? 10,
            true,
            cancellationToken);
        return Result<PaginatedListModel<OrderModel>>.Succeeded(new(ordersDataGrid.Items.ConvertAll(o => o.ToOrderModel()), ordersDataGrid.TotalItems));
    }
}

