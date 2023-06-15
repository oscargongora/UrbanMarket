namespace ChicStreetwear.Shared.Models.Order;
public class OrderProductModel
{
    public Guid ProductId { get; set; }
    public Guid? VariationId { get; set; }
    public List<OrderProductAttributeModel> Attributes { get; set; } = new();
    public string Name { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal SubTotal => Price * Quantity;
    public Guid SellerId { get; set; }
}
