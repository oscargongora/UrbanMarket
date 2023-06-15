using ChicStreetwear.Application.Common.Interfaces;
using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Aggregates.Order;
using ChicStreetwear.Domain.Aggregates.Order.Enums;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Models.Order;
using MediatR;

namespace ChicStreetwear.Application.Orders.Commands;
public sealed class UpdateOrderCommand : OrderModel, IRequest<Result<Guid>> { };

public sealed class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Result<Guid>>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IDateTimeService _timeService;

    public UpdateOrderCommandHandler(IRepository<Order> orderRepository, IDateTimeService timeService)
    {
        _orderRepository = orderRepository;
        _timeService = timeService;
    }

    public async Task<Result<Guid>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id);
        if (order is null)
        {
            return OrderErrors.OrderNotFound;
        }

        order.UpdateStatus((OrderStatus)request.Status, _timeService.Now);

        await _orderRepository.UpdateAsync(order);

        return Result<Guid>.Succeeded(order.Id);
    }
}

