namespace ChicStreetwear.Shared.Models.Product;

public class VariationModel
{
    public Guid Id { get; set; }
    public List<AttributeVariationModel> Attributes { get; set; } = new();
    public required int Stock { get; set; }
    public required decimal PurchasedPrice { get; set; }
    public required decimal RegularPrice { get; set; }
    public decimal? SalePrice { get; set; }
    public override bool Equals(object? obj)
    {
        return obj is VariationModel model &&
               Id.Equals(model.Id);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}