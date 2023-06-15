using ChicStreetwear.Shared.Interfaces;

namespace ChicStreetwear.Shared.Models.Order;
public class OrderModel : IEntityModelBase
{
    public Guid Id { get; set; }
    public Guid? CustomerId { get; set; }
    public List<OrderProductModel> Products { get; set; } = new();
    public decimal Total { get; set; } = 0;
    public DateTime PlacedDate { get; set; }
    public DateTime? DispatchedDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public AddressModel DeliveredAddress { get; set; }
    public OrderStatusModel Status { get; set; } = OrderStatusModel.Placed;
    public string PaymentIntent { get; set; }
    public string PaymentIntentClientSecret { get; set; }
    public string ReceiptEmail { get; set; }

    public decimal GetTotalByProducts => Products.Sum(p => (p.Quantity * p.Price));

    public override bool Equals(object? obj)
    {
        return obj is OrderModel model &&
               Id.Equals(model.Id);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}
