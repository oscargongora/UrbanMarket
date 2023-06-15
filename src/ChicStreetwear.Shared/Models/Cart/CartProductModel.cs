namespace ChicStreetwear.Shared.Models.Cart;
public class CartProductModel
{
    public Guid ProductId { get; set; }
    public Guid? VariationId { get; set; }
    public List<CartProductAttributeModel> Attributes { get; set; } = new();
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public Guid SellerId { get; set; }
    public string? ThumbnailUrl { get; set; }
    public int MaxQuantity { get; set; } = 1;
}
