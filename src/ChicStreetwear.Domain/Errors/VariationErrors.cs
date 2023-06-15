using ChicStreetwear.Shared;

namespace ChicStreetwear.Domain.Errors;
public static class VariationErrors
{
    public static Error NotFound => Error.NotFound("Product.VariationNotFound", "Variation not found.");
    public static Error InvalidVariationSalePrice => Error.Validation("Product.InvalidSalePrice", "Sale price for variations must be less than regular price.");
}
