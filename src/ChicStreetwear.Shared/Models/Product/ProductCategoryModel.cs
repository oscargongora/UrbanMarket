namespace ChicStreetwear.Shared.Models.Product;

public class ProductCategoryModel
{
    public Guid Id { get; set; }
    public required Guid CategoryId { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is ProductCategoryModel model &&
               Id.Equals(model.Id);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}