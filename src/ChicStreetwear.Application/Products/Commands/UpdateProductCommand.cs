using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Aggregates.Product.Entities;
using ChicStreetwear.Domain.Aggregates.Product.Enums;
using ChicStreetwear.Domain.Aggregates.Product;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Models.Product;
using MediatR;
using ChicStreetwear.Domain.ValueObjects;
using ChicStreetwear.Application.Common.Interfaces;

namespace ChicStreetwear.Application.Products.Commands;

public sealed class UpdateProductCommand : ProductModel, IRequest<Result<Guid>> { }

public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<Guid>>
{
    private readonly IRepository<Product> _productRepository;
    private readonly IDateTimeService _timeService;

    public UpdateProductCommandHandler(IRepository<Product> productRepository, IDateTimeService timeService)
    {
        _productRepository = productRepository;
        _timeService = timeService;
    }

    public async Task<Result<Guid>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);
        if (product is null)
        {
            return ProductErrors.ProductNotFound;
        }

        if (!product.SellerId.Equals(request.SellerId))
        {
            return Error.New("UpdateProduct", "The product could not be updated because you are not the owner.", Shared.Enums.ErrorType.Unauthorized);
        }

        List<ProductCategory> categories = request.Categories.ConvertAll(c => ProductCategory.New(c.CategoryId));

        var pictures = request.Pictures.ConvertAll(p => Picture.New(p.FileName, p.Name, p.Url, p.ThumbnailUrl));

        var attributes = request.Attributes.ConvertAll(a => ProductAggregate.ValueObjects.Attribute.New(a.Name));

        var variations = request.Variations.ConvertAll<(Guid?, Variation)>(v => new(v.Id, Variation.New(v.Attributes.ConvertAll(av => AttributeVariation.New(av.Value, ProductAggregate.ValueObjects.Attribute.New(av.Attribute.Name))), v.Stock, v.PurchasedPrice, v.RegularPrice, v.SalePrice)));

        var coverPicture = request.CoverPicture is null ? null : Picture.New(request.CoverPicture.FileName, request.CoverPicture.Name, request.CoverPicture.Url, request.CoverPicture.ThumbnailUrl);

        var result = product.Update(categories, attributes, variations, request.Name, request.Description,
                                    request.PurchasedPrice, request.RegularPrice, request.SalePrice, coverPicture, pictures, request.Stock, request.HasAttributes, (ProductStatus)request.Status,product.SalesAmount,_timeService.Now);

        if (result.HasErrors) return result.Errors;

        await _productRepository.UpdateAsync(result.Data, cancellationToken);

        return Result<Guid>.Succeeded(result.Data.Id);
    }
}
