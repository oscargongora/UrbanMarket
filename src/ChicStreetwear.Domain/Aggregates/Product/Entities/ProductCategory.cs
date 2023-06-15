using ChicStreetwear.Domain.Common;

namespace ChicStreetwear.Domain.Aggregates.Product.Entities;
public sealed class ProductCategory : EntityBase
{
    public Guid CategoryId { get; private set; }

    private ProductCategory() : base() { }
    private ProductCategory(Guid id, Guid categoryId) : base(id)
    {
        CategoryId = categoryId;
    }
    public static ProductCategory New(Guid categoryId)
    {
        return new(Guid.NewGuid(), categoryId);
    }
}
