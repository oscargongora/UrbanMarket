using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Application.Common.Mappers;
using ChicStreetwear.Application.Common.Queries;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Models.Order;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChicStreetwear.Application.Orders.Queries;
public sealed class GetOrderByIdQuery : GetByIdQueryBase, IRequest<Result<OrderModel>>
{
    public bool IsAdministrator { get; set; } = false;
    public Guid UserId { get; set; }
};

public sealed class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Result<OrderModel>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<OrderModel>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var orderQuery = _orderRepository
            .AsQueryable()
            .AsNoTracking();

        if (!request.IsAdministrator)
        {
            orderQuery = orderQuery.Include(o => o.Products.Where(p => p.SellerId.Equals(request.UserId)));
        }

        var order = await orderQuery.FirstOrDefaultAsync(o => o.Id.Equals(request.Id), cancellationToken);

        if (order is null)
        {
            return OrderErrors.OrderNotFound;
        }

        return Result<OrderModel>.Succeeded(order.ToOrderModel());
    }
}
