using ChicStreetwear.Domain.Aggregates.Product.ValueObjects;
using ChicStreetwear.Domain.Common;
using ChicStreetwear.Domain.ValueObjects;

namespace ChicStreetwear.Domain.Aggregates.Product.Entities;

public sealed class Variation : EntityBase
{
    private readonly List<AttributeVariation> _attributes = new();
    public IReadOnlyList<AttributeVariation> Attributes => _attributes.AsReadOnly();
    public Stock Stock { get; private set; }
    public Money PurchasedPrice { get; private set; }
    public Money RegularPrice { get; private set; }
    public Money? SalePrice { get; private set; }
    public Variation(Guid id, Stock stock, Money purchasedPrice, Money regularPrice, Money? salePrice) : base(id)
    {
        Stock = stock;
        PurchasedPrice = purchasedPrice;
        RegularPrice = regularPrice;
        SalePrice = salePrice;
    }

    public static Variation New(List<AttributeVariation> attributeVariations, int stock, decimal purchasedPrice, decimal regularPrice, decimal? salePrice)
    {
        var variation = new Variation(Guid.NewGuid(), Stock.New(stock), Money.New(purchasedPrice), Money.New(regularPrice), salePrice is null ? null : Money.New((decimal)salePrice));

        variation.AddAttributes(attributeVariations);

        return variation;
    }

    public void Update(List<AttributeVariation> attributeVariations, int stock, decimal purchasedPrice, decimal regularPrice, decimal? salePrice)
    {
        Stock = Stock.New(stock);
        PurchasedPrice = Money.New(purchasedPrice);
        RegularPrice = Money.New(regularPrice);
        SalePrice = salePrice is null ? null : Money.New((decimal)salePrice);
        UpdateAttributes(attributeVariations);
    }

    #region attribute variations
    private void AddAttributes(List<AttributeVariation> attributeVariations)
    {
        foreach (var attribute in attributeVariations)
        {
            AddAttribute(attribute);
        }
    }
    public void AddAttribute(AttributeVariation attributeVariation)
    {
        _attributes.Add(attributeVariation);
    }
    public void RemoveAttribute(ValueObjects.Attribute attribute)
    {
        _attributes.RemoveAll(_av => _av.Attribute.Equals(attribute));
    }
    private void UpdateAttributes(List<AttributeVariation> attributeVariations)
    {
        var attributesToRemove = _attributes.Where(a => !attributeVariations.Any(_a => _a.Attribute.Equals(a.Attribute))).ToList();

        foreach (var attribute in attributesToRemove)
        {
            RemoveAttribute(attribute.Attribute);
        }

        foreach (var attributeVariation in attributeVariations)
        {
            UpdateAttribute(attributeVariation);
        }
    }
    public void UpdateAttribute(AttributeVariation attributeVariation)
    {
        var _attributeVariation = _attributes.FirstOrDefault(_av => _av.Attribute.Equals(attributeVariation.Attribute));

        if (_attributeVariation is null)
        {
            AddAttribute(attributeVariation);
        }
        else
        {
            _attributeVariation.Update(attributeVariation.Value);
        }
    }

    #endregion

    #region pictures
    //public Result<Variation> UpdateCoverPicture(Picture? coverPicture)
    //{
    //    CoverPicture = coverPicture;
    //    return Result<Variation>.Succeeded(this);
    //}

    //public Result<ProductVariation> AddPicture(Picture picture)
    //{
    //    _pictures.Add(picture);
    //    return Result<ProductVariation>.Succeeded(this);
    //}

    //public Result<ProductVariation> DeletePicture(Picture picture)
    //{
    //    _pictures.Remove(picture);
    //    return Result<ProductVariation>.Succeeded(this);
    //}
    #endregion

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Variation() : base() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}