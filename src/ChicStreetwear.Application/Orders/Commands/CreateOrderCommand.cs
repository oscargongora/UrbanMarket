using ChicStreetwear.Application.Common.Interfaces;
using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Application.Common.Mappers;
using ChicStreetwear.Domain.Aggregates.Order;
using ChicStreetwear.Domain.Events.Order;
using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Models.Order;
using MediatR;

namespace ChicStreetwear.Application.Orders.Commands;

public sealed class CreateOrderCommand : OrderModel, IRequest<Result<Guid>> { }

public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<Guid>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDateTimeService _timeService;
    private readonly IPublisher _publisher;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IDateTimeService timeService, IPublisher publisher)
    {
        _orderRepository = orderRepository;
        _timeService = timeService;
        _publisher = publisher;
    }

    public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderResult = Order.New(request.CustomerId,
                                    _timeService.Now,
                                    request.DeliveredAddress.ToAddress(),
                                    request.Products.Select(p => p.ToOrderProduct()).ToList(),
                                    request.PaymentIntent,
                                    request.PaymentIntentClientSecret,
                                    request.ReceiptEmail);

        if (orderResult.HasErrors)
            return orderResult.Errors;

        await _orderRepository.AddAsync(orderResult.Data, cancellationToken);
        await _publisher.Publish(new OrderCreatedEvent() { Id = orderResult.Data.Id }, cancellationToken);
        return Result<Guid>.Succeeded(orderResult.Data.Id);
    }
}
