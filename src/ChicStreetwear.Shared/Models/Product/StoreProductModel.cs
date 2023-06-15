using ChicStreetwear.Shared.Models.Common;

namespace ChicStreetwear.Shared.Models.Product;

public record StoreAttributeVariationModel(string Name, string Value);

public class StoreProductModel
{
    public Guid ProductId { get; set; }
    public Guid? VariationId { get; set; }
    public List<StoreAttributeVariationModel> Attributes { get; set; } = new();
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsOnSale { get; set; }
    public decimal Price { get; set; }
    public string? ThumbnailUrl { get; set; }
    public RatingModel Rating { get; set; } = new() { Count = 0, Value = 0 };
}
