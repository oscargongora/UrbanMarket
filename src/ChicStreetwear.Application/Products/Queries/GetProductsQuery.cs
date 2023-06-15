using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Application.Common.Queries;
using ChicStreetwear.Application.Mappers;
using ChicStreetwear.Domain.Aggregates.Product;
using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Models.Components;
using ChicStreetwear.Shared.Models.Product;
using MediatR;
using System.Linq.Expressions;

namespace ChicStreetwear.Application.Products.Queries;
public sealed class GetProductsQuery : GetQueryBase, IRequest<Result<PaginatedListModel<ProductModel>>> { }

public sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<PaginatedListModel<ProductModel>>>
{
    private readonly IRepository<Product> _productRepository;
    public GetProductsQueryHandler(IRepository<Product> ProductRepository)
    {
        _productRepository = ProductRepository;
    }

    public async Task<Result<PaginatedListModel<ProductModel>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var search = request.Search ?? string.Empty;
        Expression<Func<Product, bool>> predicate = product => product.Name.Contains(search);
        List<Expression<Func<Product, bool>>> predicates = new() { predicate };
        predicates.Add(p => !p.Variations.Any());

        var products = await _productRepository.PaginatedListAsync(predicates, new(), request.Page ?? 1, request.Take ?? 10, true, cancellationToken);

        return Result<PaginatedListModel<ProductModel>>.Succeeded(new(products.Items.ConvertAll(p => p.ToProductModel()), products.TotalItems));
    }
}
