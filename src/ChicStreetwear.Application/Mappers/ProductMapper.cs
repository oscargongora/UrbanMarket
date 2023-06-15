using ChicStreetwear.Domain.Aggregates.Product;
using ChicStreetwear.Domain.Aggregates.Product.ValueObjects;
using ChicStreetwear.Domain.ValueObjects;
using ChicStreetwear.Shared.Models.Common;
using ChicStreetwear.Shared.Models.Product;
using Riok.Mapperly.Abstractions;

namespace ChicStreetwear.Application.Mappers;

[Mapper]
internal static partial class ProductMapper
{
    internal static partial ProductModel ToProductModel(this Product product);
    internal static StoreProductModel ToStoreProductModel(this Product product)
    {
        StoreProductModel storeProduct = new()
        {
            ProductId = product.Id,
            Name = product.Name,
            Description = product.Description,
            IsOnSale = product.SalePrice is not null,
            Price = product.SalePrice?.Amount ?? product.RegularPrice?.Amount ?? 0,
            ThumbnailUrl = product.CoverPicture?.ThumbnailUrl,
            Rating = product.Rating.ToRatingModel()
        };

        if (product.HasAttributes)
        {
            var variation = product.Variations.FirstOrDefault(v => v.Stock.Quantity > 0);
            if (variation is not null)
            {
                storeProduct.VariationId = variation.Id;
                storeProduct.Attributes = variation.Attributes.Select(va => new StoreAttributeVariationModel(va.Attribute.Name, va.Value)).ToList();
                storeProduct.IsOnSale = variation.SalePrice is not null;
                storeProduct.Price = variation.SalePrice?.Amount ?? variation.RegularPrice.Amount;
            }
        }
        return storeProduct;
    }
    private static int StockToInt(Stock stock) => stock.Quantity;
    private static decimal MoneyToDecimal(Money money) => money.Amount;
    internal static partial RatingModel ToRatingModel(this Rating product);
}
