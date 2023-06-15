using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Aggregates.Product;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Models.Cart;
using MediatR;
using System.Linq.Expressions;

namespace ChicStreetwear.Application.Products.Queries;

public sealed record CartProductQueryModel(Guid ProductId, Guid? VariationId, int Quantity);

public sealed class GetCartProductsQuery : IRequest<Result<IEnumerable<CartProductModel>>>
{
    public required List<CartProductQueryModel> Products { get; set; }
}

public sealed class GetCartProductsQueryHandler : IRequestHandler<GetCartProductsQuery, Result<IEnumerable<CartProductModel>>>
{
    private readonly IProductRepository _productRepository;

    public GetCartProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<IEnumerable<CartProductModel>>> Handle(GetCartProductsQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Product, bool>> predicate = p => request.Products.Select(p => p.ProductId).Contains(p.Id);

        var products = await _productRepository.ListAsync(predicate, true, cancellationToken);

        if (products is null)
        {
            return ProductErrors.ProductNotFound;
        }
        List<CartProductModel> cartProducts = new();

        foreach (var rp in request.Products)
        {
            var product = products.FirstOrDefault(p => p.Id.Equals(rp.ProductId));

            if (product is not null)
            {
                CartProductModel cartProduct = new()
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Quantity = rp.Quantity,
                    Price = product.SalePrice?.Amount ?? product.RegularPrice?.Amount ?? 0,
                    SellerId = product.SellerId,
                    ThumbnailUrl = product.CoverPicture?.ThumbnailUrl,
                    MaxQuantity = product.Stock.Quantity
                };

                if (rp.VariationId is not null)
                {
                    var variation = product.Variations.FirstOrDefault(v => v.Id.Equals(rp.VariationId));
                    if (variation is not null)
                    {
                        cartProduct.VariationId = variation.Id;
                        cartProduct.Attributes = variation.Attributes.Select(va => new CartProductAttributeModel() { Name = va.Attribute.Name, Value = va.Value }).ToList();
                        cartProduct.Price = variation.SalePrice?.Amount ?? variation.RegularPrice.Amount;
                        cartProduct.MaxQuantity = variation.Stock.Quantity;
                    }
                }
                cartProducts.Add(cartProduct);
            }
        }

        return Result<IEnumerable<CartProductModel>>.Succeeded(cartProducts);
    }
}