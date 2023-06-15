using ChicStreetwear.Domain.Aggregates.Order.Entities;
using ChicStreetwear.Domain.Aggregates.Order.Enums;
using ChicStreetwear.Domain.Common;
using ChicStreetwear.Domain.Common.Interfaces;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Domain.ValueObjects;
using ChicStreetwear.Shared;

namespace ChicStreetwear.Domain.Aggregates.Order;
public sealed class Order : EntityBase, IAggregateRoot
{
    private readonly List<OrderProduct> _products = new();
    public Guid? CustomerId { get; private set; }
    public IReadOnlyList<OrderProduct> Products => _products.AsReadOnly();
    public Money Total { get; private set; } = Money.New(0);
    public DateTime PlacedDate { get; private set; }
    public DateTime? DispatchedDate { get; private set; }
    public DateTime? DeliveredDate { get; private set; }
    public Address DeliveredAddress { get; private set; }
    public OrderStatus Status { get; private set; } = OrderStatus.Placed;
    public string PaymentIntent { get; private set; }
    public string PaymentIntentClientSecret { get; private set; }
    public string ReceiptEmail { get; private set; }

    private Order(Guid id, Guid? customerId, DateTime placedDate, Address deliveredAddress, string paymentIntent, string paymentIntentClientSecret, string receiptEmail) : base(id)
    {
        CustomerId = customerId;
        PlacedDate = placedDate;
        DeliveredAddress = deliveredAddress;
        PaymentIntent = paymentIntent;
        PaymentIntentClientSecret = paymentIntentClientSecret;
        ReceiptEmail = receiptEmail;
    }

    public static Result<Order> New(Guid? customerId, DateTime placedDate, Address deliveredAddress, List<OrderProduct> products, string paymentIntent, string paymentIntentClientSecret, string receiptEmail)
    {
        Order order = new Order(Guid.NewGuid(),
                                customerId,
                                placedDate,
                                deliveredAddress,
                                paymentIntent,
                                paymentIntentClientSecret,
                                receiptEmail);
        order.AddProducts(products);
        return Result<Order>.Succeeded(order);
    }

    private Result<Order> AddProducts(List<OrderProduct> products)
    {
        foreach (var product in products)
        {
            var result = AddProduct(product);
            if (result.HasErrors) return result;
        }
        return Result<Order>.Succeeded(this);
    }

    private Result<Order> AddProduct(OrderProduct product)
    {
        if (product.Price.Amount < 0)
            return MoneyErrors.InvalidAmount(nameof(Order.AddProduct));
        if (product.Quantity < 1)
            return OrderErrors.InvalidProductQuantity(nameof(Order.AddProduct));
        _products.Add(product);
        Total = Money.New(Total.Amount + (product.Price.Amount * product.Quantity));
        return Result<Order>.Succeeded(this);
    }

    public Result<Order> SetPlaced(DateTime date)
    {
        Status = OrderStatus.Placed;
        PlacedDate = date;
        return Result<Order>.Succeeded(this);
    }

    public Result<Order> SetDispatched(DateTime date)
    {
        Status = OrderStatus.Dispatched;
        DispatchedDate = date;
        return Result<Order>.Succeeded(this);
    }

    public Result<Order> SetDelivered(DateTime date)
    {
        Status = OrderStatus.Delivered;
        DeliveredDate = date;
        return Result<Order>.Succeeded(this);
    }

    public void UpdateStatus(OrderStatus status, DateTime date)
    {
        if (status == OrderStatus.Dispatched)
        {
            Status = OrderStatus.Dispatched;
            DispatchedDate = date;
        }
        if (status == OrderStatus.Delivered)
        {
            Status = OrderStatus.Delivered;
            DeliveredDate = date;
        }
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Order() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
