using ChicStreetwear.Domain.Aggregates.Product.Entities;
using ChicStreetwear.Domain.Aggregates.Product.Enums;
using ChicStreetwear.Domain.Aggregates.Product.ValueObjects;
using ChicStreetwear.Domain.Common;
using ChicStreetwear.Domain.Common.Interfaces;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Domain.ValueObjects;
using ChicStreetwear.Shared;

namespace ChicStreetwear.Domain.Aggregates.Product;
public sealed class Product : EntityBase, IAggregateRoot
{
    private readonly List<ProductCategory> _categories = new();

    private readonly List<Picture> _pictures = new();

    private readonly List<ValueObjects.Attribute> _attributes = new();

    private readonly List<Variation> _variations = new();

    public string Name { get; private set; }
    public string Description { get; private set; }
    public Money? PurchasedPrice { get; private set; }
    public Money? RegularPrice { get; private set; }
    public Money? SalePrice { get; private set; }
    public IReadOnlyList<ProductCategory> Categories => _categories.AsReadOnly();
    public Picture? CoverPicture { get; private set; }
    public IReadOnlyList<Picture> Pictures => _pictures.AsReadOnly();
    public Stock Stock { get; private set; }
    public Rating Rating { get; private set; } = Rating.New();
    public bool HasAttributes { get; private set; }
    public IReadOnlyList<ValueObjects.Attribute> Attributes => _attributes.AsReadOnly();
    public IReadOnlyList<Variation> Variations => _variations.AsReadOnly();
    public ProductStatus Status { get; private set; } = ProductStatus.Draft;
    public Guid SellerId { get; private set; }
    public int SalesAmount { get; private set; } = 0;
    public DateTime CreatedDate { get; private set; }
    public DateTime? UpdatedDate { get; private set; }

    public Product(Guid id, string name, string description, Money? purchasedPrice, Money? regularPrice, Money? salePrice, Picture? coverPicture, Stock stock, bool hasAttributes, ProductStatus status, Guid sellerId, DateTime createdDate) : base(id)
    {
        Name = name;
        Description = description;
        PurchasedPrice = purchasedPrice;
        RegularPrice = regularPrice;
        SalePrice = salePrice;
        CoverPicture = coverPicture;
        Stock = stock;
        HasAttributes = hasAttributes;
        Status = status;
        SellerId = sellerId;
        CreatedDate = createdDate;
    }

    public static Result<Product> New(List<ProductCategory> categories, List<ValueObjects.Attribute>? attributes, List<Variation>? variations, string name, string description, decimal? purchasedPrice, decimal? regularPrice, decimal? salePrice, Picture? coverPicture, List<Picture>? pictures, int? stock, bool hasAttributes, ProductStatus status, Guid sellerId, DateTime createdDate)
    {
        var product = new Product(Guid.NewGuid(),
                                  name,
                                  description,
                                  null,
                                  null,
                                  null,
                                  coverPicture,
                                  Stock.New(0),
                                  hasAttributes,
                                  status,
                                  sellerId,
                                  createdDate);

        var result = product.AddCategories(categories);
        if (result.HasErrors)
            return result;

        result = product.AddPictures(pictures ?? new List<Picture>());
        if (result.HasErrors)
            return result;

        result = product.AddAttributes(attributes ?? new List<ValueObjects.Attribute>());
        if (result.HasErrors)
            return result;

        result = product.AddVariations(variations ?? new List<Variation>());
        if (result.HasErrors)
            return result;

        result = product.UpdatePurchasedPrice(purchasedPrice is null ? null : Money.New((decimal)purchasedPrice));
        if (result.HasErrors)
            return result;

        result = product.UpdateRegularPrice(regularPrice is null ? null : Money.New((decimal)regularPrice));
        if (result.HasErrors)
            return result;

        result = product.UpdateSalePrice(salePrice is null ? null : Money.New((decimal)salePrice));
        if (result.HasErrors)
            return result;

        result = product.UpdateStock(stock is null ? null : Stock.New((int)stock));
        if (result.HasErrors)
            return result;

        return result;
    }

    public Result<Product> Update(List<ProductCategory> categories, List<ValueObjects.Attribute>? attributes, List<(Guid? variationId, Variation variation)>? variations, string name, string description, decimal? purchasedPrice, decimal? regularPrice, decimal? salePrice, Picture? coverPicture, List<Picture>? pictures, int? stock, bool hasAttributes, ProductStatus status, int salesAmount, DateTime updatedDate)
    {
        Name = name;
        Description = description;
        CoverPicture = coverPicture;
        Status = status;
        SalesAmount = salesAmount;
        UpdatedDate = updatedDate;

        if (!hasAttributes.Equals(HasAttributes))
        {
            Stock = Stock.New(0);
            _attributes.Clear();
            _variations.Clear();
        }

        //important change first HasAttributes
        HasAttributes = hasAttributes;

        var result = UpdateCategories(categories);
        if (result.HasErrors)
            return result;

        result = UpdatePictures(pictures ?? new());
        if (result.HasErrors)
            return result;

        result = UpdateAttributes(attributes ?? new());
        if (result.HasErrors)
            return result;

        result = UpdateVariations(variations ?? new List<(Guid? variationId, Variation variation)>());
        if (result.HasErrors)
            return result;

        result = UpdatePurchasedPrice(purchasedPrice is null ? null : Money.New((decimal)purchasedPrice));
        if (result.HasErrors)
            return result;

        result = UpdateRegularPrice(regularPrice is null ? null : Money.New((decimal)regularPrice));
        if (result.HasErrors)
            return result;

        result = UpdateSalePrice(salePrice is null ? null : Money.New((decimal)salePrice));
        if (result.HasErrors)
            return result;

        result = UpdateStock(stock is null ? null : Stock.New((int)stock));
        if (result.HasErrors)
            return result;

        return result;
    }

    public Result<Product> IncreaseSalesAmount(Guid? variationId, int increaseNumber, DateTime updatedDate)
    {
        if (increaseNumber < 1) return Error.BadRequest("Product.IncreaseSalesAmount");
        SalesAmount += increaseNumber;
        Stock = Stock.New(Stock.Quantity - increaseNumber);
        if (variationId is not null)
        {
            var variation = Variations.FirstOrDefault(v => v.Id.Equals(variationId));
            if (variationId is null) return Error.BadRequest("Product.IncreaseSalesAmount");
            variation.Update(variation.Attributes.ToList(), variation.Stock.Quantity - increaseNumber, variation.PurchasedPrice.Amount, variation.RegularPrice.Amount, variation.SalePrice?.Amount);
        }
        UpdatedDate = updatedDate;
        return Result<Product>.Succeeded(this);
    }

    #region categories
    private Result<Product> AddCategories(List<ProductCategory> categories)
    {
        foreach (var category in categories)
        {
            var result = AddCategory(category);
            if (result.HasErrors) return result;
        }
        return Result<Product>.Succeeded(this);
    }
    public Result<Product> AddCategory(ProductCategory category)
    {
        _categories.Add(category);
        return Result<Product>.Succeeded(this);
    }
    public Result<Product> RemoveCategory(Guid categoryId)
    {
        var category = _categories.FirstOrDefault(c => c.CategoryId == categoryId);
        if (category is null) return CategoryErrors.NotFound;
        _categories.Remove(category);
        return Result<Product>.Succeeded(this);
    }
    private Result<Product> UpdateCategories(List<ProductCategory> categories)
    {
        var categoriesToRemove = _categories.Where(pc => !categories.Any(c => c.CategoryId == pc.CategoryId)).Select(pc => pc.CategoryId).ToList();

        foreach (var categoryId in categoriesToRemove)
        {
            var result = RemoveCategory(categoryId);
            if (result.HasErrors) return result;
        }

        foreach (var category in categories)
        {
            var _category = _categories.FirstOrDefault(_c => _c.CategoryId == category.CategoryId);
            if (_category is null)
            {
                var result = AddCategory(category);
                if (result.HasErrors) return result;
            }
        }
        return Result<Product>.Succeeded(this);
    }
    #endregion

    #region pictures
    private Result<Product> AddPictures(List<Picture> pictures)
    {
        foreach (var picture in pictures)
        {
            var result = AddPicture(picture);
            if (result.HasErrors) return result;
        }
        return Result<Product>.Succeeded(this);
    }
    public Result<Product> AddPicture(Picture picture)
    {
        if (HasAttributes)
        {
            return ProductErrors.InvalidFunctionalityForProductWithAttributes(nameof(AddPicture));
        }
        _pictures.Add(picture);
        return Result<Product>.Succeeded(this);
    }
    public Result<Product> RemovePicture(string fileName)
    {
        var picture = _pictures.FirstOrDefault(p => p.FileName.Equals(fileName));
        if (picture is null)
        {
            return PictureErrors.NotFound;
        }
        if (CoverPicture is not null && CoverPicture.Url.Equals(picture.Url))
        {
            CoverPicture = null;
        }
        _pictures.Remove(picture);
        return Result<Product>.Succeeded(this);
    }
    private Result<Product> UpdatePictures(List<Picture> pictures)
    {
        var picturesToRemove = _pictures.Where(p => !pictures.Any(_p => _p.Equals(p)))
                                            .ToList();

        foreach (var picture in picturesToRemove)
        {
            var result = _pictures.Remove(picture);
        }

        foreach (var picture in pictures)
        {
            if (!_pictures.Contains(picture))
            {
                var result = AddPicture(picture);
                if (result.HasErrors) return result;
            }
        }
        return Result<Product>.Succeeded(this);
    }
    #endregion

    #region attributes
    private bool AnyAttribute(string attributeName)
    {
        return _attributes.Any(attribute => attribute.Name == attributeName);
    }
    private Result<Product> AddAttributes(List<ValueObjects.Attribute> attributes)
    {
        foreach (var attribute in attributes)
        {
            var result = AddAttribute(attribute);
            if (result.HasErrors) return result;
        }
        return Result<Product>.Succeeded(this);
    }
    private Result<Product> AddAttribute(ValueObjects.Attribute attribute)
    {
        if (!HasAttributes)
        {
            return ProductErrors.InvalidFunctionalityForProductWithoutAttributes(nameof(AddAttribute));
        }
        _attributes.Add(attribute);
        return Result<Product>.Succeeded(this);
    }
    private Result<Product> RemoveAttribute(ValueObjects.Attribute attribute)
    {
        if (!HasAttributes)
        {
            return ProductErrors.InvalidFunctionalityForProductWithoutAttributes(nameof(RemoveAttribute));
        }

        var result = _attributes.Remove(attribute);

        return Result<Product>.Succeeded(this);
    }
    private Result<Product> UpdateAttributes(List<ValueObjects.Attribute> attributes)
    {
        var attributesToRemove = _attributes.Where(a => !attributes.Any(_a => _a.Equals(a)))
                                            .ToList();

        foreach (var attribute in attributesToRemove)
        {
            var result = RemoveAttribute(attribute);
            if (result.HasErrors) return result;
        }

        foreach (var attribute in attributes)
        {
            var _attribute = _attributes.FirstOrDefault(_a => _a.Equals(attribute));
            if (_attribute is null)
            {
                var result = AddAttribute(attribute);
                if (result.HasErrors) return result;
            }
        }
        return Result<Product>.Succeeded(this);
    }
    #endregion

    #region variations
    private Result<Variation> ValidateVariation(Variation variation)
    {
        if (variation.PurchasedPrice.Amount < 0 || variation.RegularPrice.Amount < 0)
            return MoneyErrors.InvalidAmount(nameof(Product.ValidateVariation));

        if (variation.SalePrice is not null && variation.SalePrice.Amount >= variation.RegularPrice.Amount)
            return VariationErrors.InvalidVariationSalePrice;
        if (variation.SalePrice is not null && variation.SalePrice.Amount < 0)
            return MoneyErrors.InvalidAmount(nameof(Product.ValidateVariation));

        if (variation.Stock.Quantity < 0)
            return StockErrors.InvalidQuantity(nameof(Product.ValidateVariation));

        return Result<Variation>.Succeeded(variation);
    }
    private Result<Product> AddVariations(List<Variation> variations)
    {
        foreach (var variation in variations)
        {
            var result = AddVariation(variation);
            if (result.HasErrors)
                return result;
        }
        return Result<Product>.Succeeded(this);
    }
    public Result<Product> AddVariation(Variation variation)
    {
        if (!HasAttributes)
            return ProductErrors.InvalidFunctionalityForProductWithoutAttributes(nameof(AddVariation));

        var result = ValidateVariation(variation);
        if (result.HasErrors) return result.Errors;

        if (CheckForMissingAttributes(_attributes, variation))
        {
            return ProductErrors.MissingAttributesValues;
        }

        _variations.Add(variation);
        Stock = Stock.New(Stock.Quantity + variation.Stock.Quantity);
        return Result<Product>.Succeeded(this);
    }
    public Result<Product> RemoveVariation(Guid variationId)
    {
        if (!HasAttributes)
        {
            return ProductErrors.InvalidFunctionalityForProductWithoutAttributes(nameof(RemoveVariation));
        }
        var variation = _variations.FirstOrDefault(variation => variation.Id == variationId);
        if (variation is null)
        {
            return VariationErrors.NotFound;
        }
        _variations.Remove(variation);
        Stock = Stock.New(Stock.Quantity - variation.Stock.Quantity);
        return Result<Product>.Succeeded(this);
    }
    private Result<Product> UpdateVariation(Guid? variationId, Variation variation)
    {
        if (!HasAttributes)
            return ProductErrors.InvalidFunctionalityForProductWithoutAttributes(nameof(Product.UpdateVariation));

        var _variation = _variations.FirstOrDefault(_v => _v.Id == variationId);

        if (_variation is null)
        {
            var result = AddVariation(variation);
            if (result.HasErrors) return result;
        }
        else
        {
            var variationResult = ValidateVariation(variation);
            if (variationResult.HasErrors)
                return variationResult.Errors;

            if (CheckForMissingAttributes(_attributes, variation))
                return ProductErrors.MissingAttributesValues;

            Stock = Stock.New(Stock.Quantity - _variation.Stock.Quantity + variation.Stock.Quantity);

            _variation.Update(variation.Attributes.ToList(), variation.Stock.Quantity, variation.PurchasedPrice.Amount, variation.RegularPrice.Amount, variation.SalePrice?.Amount);
        }
        return Result<Product>.Succeeded(this);
    }
    private Result<Product> UpdateVariations(List<(Guid? variationId, Variation variation)> variations)
    {
        var variationsToRemove = _variations.Where(a => !variations.Any(_a => _a.variationId == a.Id))
                                            .Select(_a => _a.Id).ToList();

        foreach (var variationId in variationsToRemove)
        {
            var result = RemoveVariation(variationId);
            if (result.HasErrors) return result;
        }

        foreach (var (variationId, variation) in variations)
        {
            var result = UpdateVariation(variationId, variation);
            if (result.HasErrors) return result;
        }
        return Result<Product>.Succeeded(this);
    }
    #endregion

    #region prices
    public Result<Product> UpdatePurchasedPrice(Money? purchasedPrice)
    {
        if (HasAttributes)
        {
            PurchasedPrice = null;
        }
        else
        {
            if (purchasedPrice is null)
                return Error.Validation(nameof(Product.UpdatePurchasedPrice), "The purchase price is required for products without attributes.");

            if (purchasedPrice!.Amount < 0)
                return MoneyErrors.InvalidAmount(nameof(Product.UpdatePurchasedPrice));

            PurchasedPrice = purchasedPrice;
        }
        return Result<Product>.Succeeded(this);
    }
    public Result<Product> UpdateRegularPrice(Money? regularPrice)
    {
        if (HasAttributes)
        {
            RegularPrice = null;
        }
        else
        {
            if (regularPrice is null)
                return Error.Validation(nameof(Product.UpdateRegularPrice), "The regular price is required for products without attributes.");

            if (regularPrice!.Amount < 0)
                return MoneyErrors.InvalidAmount(nameof(Product.UpdateRegularPrice));

            SalePrice = null;
            RegularPrice = regularPrice;
        }
        return Result<Product>.Succeeded(this);
    }
    public Result<Product> UpdateSalePrice(Money? salePrice)
    {
        if (HasAttributes)
        {
            SalePrice = null;
        }
        else
        {
            if (salePrice is not null)
            {
                if (salePrice.Amount < 0)
                    return MoneyErrors.InvalidAmount(nameof(Product.UpdateSalePrice));

                if (RegularPrice is null)
                    return Error.BadRequest("Product.UpdateSalePrice", "To update the sale price you must establish a regular price.");

                if (RegularPrice is not null && salePrice.Amount >= RegularPrice.Amount)
                    return ProductErrors.InvalidProductSalePrice(nameof(Product.New));
            }

            SalePrice = salePrice;
        }
        return Result<Product>.Succeeded(this);
    }
    #endregion

    public Result<Product> UpdateStock(Stock? stock)
    {
        if (!HasAttributes)
        {
            if (stock is null)
                return ProductErrors.InvalidStockForProductWithoutAttributes;

            if (stock.Quantity < 0)
                return StockErrors.InvalidQuantity(nameof(Product.UpdateStock));

            Stock = stock;
        }

        return Result<Product>.Succeeded(this);
    }

    public Result<Product> UpdateCoverPicture(Picture coverPicture)
    {
        CoverPicture = coverPicture;
        return Result<Product>.Succeeded(this);
    }

    public Result<Product> AddRating(int rating)
    {
        if (rating < 1 || rating > 5) return RatingErrors.InvalidValue;
        var count = Rating.Count + 1;
        var value = ((Rating.Value * Rating.Count) + rating) / count;
        Rating = Rating.New(value, count);
        return Result<Product>.Succeeded(this);
    }

    public Result<Product> RemoveRating(int rating)
    {
        if (rating < 1 || rating > 5) return RatingErrors.InvalidValue;
        var count = Rating.Count - 1;
        var value = ((Rating.Value * Rating.Count) - rating) / count;
        Rating = Rating.New(value, count);
        return Result<Product>.Succeeded(this);
    }

    public void SetActiveStatus()
    {
        Status = ProductStatus.Active;
    }
    public void SetDraftStatus()
    {
        Status = ProductStatus.Draft;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Product() : base() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private bool CheckForMissingAttributes(List<ValueObjects.Attribute> attributes, Variation variation)
    {
        if (variation.Attributes.Count != attributes.Count)
            return true;

        foreach (var attribute in attributes)
            if (!variation.Attributes.Any(attributeValue => attributeValue.Attribute.Equals(attribute)))
                return true;

        return false;
    }
}
