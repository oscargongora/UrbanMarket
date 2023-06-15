using ChicStreetwear.Shared;

namespace ChicStreetwear.Domain.Errors;
public static class ProductErrors
{
    public static Error ProductNotFound => Error.NotFound("Product.NotFound", "Product not found.");


    #region prices
    public static Error InvalidProductSalePrice(string key) => Error.Validation("Product.InvalidSalePrice", "Sale price for products must be less than regular price.");
    #endregion

    #region attributes
    public static Error MissingAttributesValues => Error.Validation("Product.MissingAttributesValues", "There are missing values for one or more attributes in the variations.");

    public static Error AttributeNotFound => Error.NotFound("Product.AttributeNotFound", "Attribute not found.");
    #endregion

    public static Error InvalidFunctionalityForProductWithAttributes(string nameOfFunctionality) => Error.New("Product.InvalidFunctionalityForProductWithAttributes", $"Products with attributes do not have {nameOfFunctionality} property.", Shared.Enums.ErrorType.BadRequest);

    public static Error InvalidFunctionalityForProductWithoutAttributes(string nameOfFunctionality) => Error.New("Product.InvalidFunctionalityForProductWithoutAttributes", $"Products without attributes do not have {nameOfFunctionality} property.", Shared.Enums.ErrorType.BadRequest);

    public static Error FailedRemovingTheUniqueProductAttribute(string attributeName) => Error.BadRequest("Product.DeleteAttribute", $"The attribute {attributeName} cannot be removed because it is the only attribute that your product has, if you want to remove it you must add another attribute or change to product without attribute.");

    #region stock
    public static Error InvalidStockForProductWithoutAttributes => Error.BadRequest("Product.InvalidStockForProductWithoutAttributes", "The stock is required for products without attributes.");

    public static Error VariationNotFound => Error.NotFound("Product.VariationNotFound", "Variation not found.");
    #endregion
}
