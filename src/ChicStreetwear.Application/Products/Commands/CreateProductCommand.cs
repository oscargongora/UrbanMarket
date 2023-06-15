using ChicStreetwear.Application.Common.Interfaces;
using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Aggregates.Product;
using ChicStreetwear.Domain.Aggregates.Product.Entities;
using ChicStreetwear.Domain.Aggregates.Product.Enums;
using ChicStreetwear.Domain.ValueObjects;
using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Models.Product;
using MediatR;

namespace ChicStreetwear.Application.Products.Commands;
public sealed class CreateProductCommand : ProductModel, IRequest<Result<Guid>> { }

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IRepository<Product> _productRepository;
    private readonly IDateTimeService _timeService;

    public CreateProductCommandHandler(IRepository<Product> productRepository, IDateTimeService timeService)
    {
        _productRepository = productRepository;
        _timeService = timeService;
    }

    public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var categories = request.Categories.ConvertAll(c => ProductCategory.New(c.CategoryId));

        var pictures = request.Pictures.ConvertAll(p => Picture.New(p.FileName, p.Name, p.Url, p.ThumbnailUrl));

        var attributes = request.Attributes.ConvertAll(a => ProductAggregate.ValueObjects.Attribute.New(a.Name));

        var variations = request.Variations.ConvertAll(v => Variation.New(v.Attributes.ConvertAll(av => AttributeVariation.New(av.Value, ProductAggregate.ValueObjects.Attribute.New(av.Attribute.Name))),
                                                                           v.Stock,
                                                                           v.PurchasedPrice,
                                                                           v.RegularPrice,
                                                                           v.SalePrice));

        var coverPicture = request.CoverPicture is null ? null : Picture.New(request.CoverPicture.FileName, request.CoverPicture.Name, request.CoverPicture.Url, request.CoverPicture.ThumbnailUrl);

        var result = Product.New(categories, attributes, variations, request.Name, request.Description, request.PurchasedPrice, request.RegularPrice, request.SalePrice, coverPicture, pictures, request.Stock, request.HasAttributes, (ProductStatus)request.Status, request.SellerId, _timeService.Now);

        if (result.HasErrors) return result.Errors;

        await _productRepository.AddAsync(result.Data, cancellationToken);

        return Result<Guid>.Succeeded(result.Data.Id);
    }
}
