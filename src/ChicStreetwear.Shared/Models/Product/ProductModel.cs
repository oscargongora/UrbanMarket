using ChicStreetwear.Shared.Interfaces;
using ChicStreetwear.Shared.Models.Common;

namespace ChicStreetwear.Shared.Models.Product;
public class ProductModel : IEntityModelBase
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal? PurchasedPrice { get; set; }
    public decimal? RegularPrice { get; set; }
    public decimal? SalePrice { get; set; }
    public List<ProductCategoryModel> Categories { get; set; } = new();
    public PictureModel? CoverPicture { get; set; }
    public List<PictureModel> Pictures { get; set; } = new();
    public int? Stock { get; set; }
    public RatingModel Rating { get; set; } = new() { Count = 0, Value = 0 };
    public bool HasAttributes { get; set; }
    public List<AttributeModel> Attributes { get; set; } = new();
    public List<VariationModel> Variations { get; set; } = new();
    public ProductStatusModel Status { get; set; } = ProductStatusModel.Draft;
    public Guid SellerId { get; set; } = Guid.Empty;

    public override bool Equals(object? obj)
    {
        return obj is ProductModel model &&
               Id.Equals(model.Id);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}