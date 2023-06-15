using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Application.Common.Mappers;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Models.Order;
using MediatR;

namespace ChicStreetwear.Application.Orders.Queries;
public sealed class GetOrderByPaymentIntentQuery : IRequest<Result<OrderModel>>
{
    public required string PaymentIntent { get; set; }
    public required string PaymentIntentClientSecret { get; set; }
};

public sealed class GetOrderByPaymentIntentQueryHandler : IRequestHandler<GetOrderByPaymentIntentQuery, Result<OrderModel>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByPaymentIntentQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<OrderModel>> Handle(GetOrderByPaymentIntentQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.FirstOrDefaultAsync(o => 
        o.PaymentIntent.Equals(request.PaymentIntent) && 
        o.PaymentIntentClientSecret.Equals(request.PaymentIntentClientSecret), 
        true, cancellationToken);

        if (order is null)
        {
            return OrderErrors.OrderNotFound;
        }
        return Result<OrderModel>.Succeeded(order.ToOrderModel());
    }
}
