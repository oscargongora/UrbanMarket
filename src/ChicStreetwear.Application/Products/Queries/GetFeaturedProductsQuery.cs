using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Application.Mappers;
using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Models.Product;
using MediatR;

namespace ChicStreetwear.Application.Products.Queries;
public sealed class GetFeaturedProductsQuery : IRequest<Result<IEnumerable<StoreProductModel>>>
{
    public int Take { get; set; } = 4;
};

public sealed class GetFeaturedProductsQueryHandler : IRequestHandler<GetFeaturedProductsQuery, Result<IEnumerable<StoreProductModel>>>
{
    private readonly IProductRepository _productRepository;

    public GetFeaturedProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<IEnumerable<StoreProductModel>>> Handle(GetFeaturedProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.ListFeaturedProducts(request.Take);
        if (products is null)
        {
            return Error.New("GetFeaturedProductsQueryHandler", "An error ocurred while getting the products.", Shared.Enums.ErrorType.Internal);
        }

        return Result<IEnumerable<StoreProductModel>>.Succeeded(products.Select(p => p.ToStoreProductModel()));
    }
}
