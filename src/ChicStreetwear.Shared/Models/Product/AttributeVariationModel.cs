namespace ChicStreetwear.Shared.Models.Product;

public class AttributeVariationModel
{
    public Guid Id { get; set; }
    public required string Value { get; set; }
    public required AttributeModel Attribute { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is AttributeVariationModel model &&
               Id.Equals(model.Id);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}