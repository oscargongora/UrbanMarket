using ChicStreetwear.Application.Common.Interfaces;
using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Events.Order;
using MediatR;

namespace ChicStreetwear.Application.Orders.EventHandlers;
public sealed class OrderCreatedEventHandler : INotificationHandler<OrderCreatedEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IDateTimeService _timeService;

    public OrderCreatedEventHandler(IOrderRepository orderRepository, IProductRepository productRepository, IDateTimeService timeService)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _timeService = timeService;
    }

    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(notification.Id);
        if (order is not null)
        {
            foreach (var orderProduct in order.Products)
            {
                var product = await _productRepository.GetByIdAsync(orderProduct.ProductId);
                if (product is not null)
                {
                    product.IncreaseSalesAmount(orderProduct.VariationId, orderProduct.Quantity, _timeService.Now);
                }
            }
            await _productRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
