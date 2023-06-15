using ChicStreetwear.Shared.Interfaces;

namespace ChicStreetwear.Shared.Models.Category;

public class  CategoryModel : IEntityModelBase
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is CategoryModel model &&
               Id.Equals(model.Id);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}
