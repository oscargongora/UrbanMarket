using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Application.Common.Queries;
using ChicStreetwear.Application.Mappers;
using ChicStreetwear.Domain.Aggregates.Product;
using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Models.Product;
using MediatR;

namespace ChicStreetwear.Application.Products.Queries;
public sealed class GetProductByIdQuery : GetByIdQueryBase, IRequest<Result<ProductModel>> { }

public sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductModel>>
{
    private readonly IRepository<Product> _productRepository;

    public GetProductByIdQueryHandler(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<ProductModel>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null) return Error.NotFound(nameof(GetProductByIdQueryHandler.Handle), "Product not found.");

        return Result<ProductModel>.Succeeded(product.ToProductModel());
    }
}