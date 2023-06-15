using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Application.Common.Queries;
using ChicStreetwear.Application.Mappers;
using ChicStreetwear.Domain.Aggregates.Product;
using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Models.Components;
using ChicStreetwear.Shared.Models.Product;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace ChicStreetwear.Application.Products.Queries;

public sealed class GetProductsDataGridQuery : GetQueryBase, IRequest<Result<PaginatedListModel<ProductModel>>>
{
    public Guid SellerId { get; set; }
}

public sealed class GetProductsDataGridQueryHandler : IRequestHandler<GetProductsDataGridQuery, Result<PaginatedListModel<ProductModel>>>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<GetProductsDataGridQueryHandler> _logger;

    public GetProductsDataGridQueryHandler(IProductRepository productRepository, ILogger<GetProductsDataGridQueryHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<Result<PaginatedListModel<ProductModel>>> Handle(GetProductsDataGridQuery request, CancellationToken cancellationToken)
    {
        List<Expression<Func<Product, bool>>> predicates = new() { p => p.SellerId.Equals(request.SellerId) };

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            predicates.Add(p => p.Name.Contains(request.Search));
            predicates.Add(p => p.Description.Contains(request.Search));
        }
        predicates.Add(p => !p.Variations.Any());
        var productsDataGrid = await _productRepository.PaginatedListAsync(
            predicates,
            new(),
            request.Sorts,
            request.Filters,
            request.Page ?? 1,
            request.Take ?? 10,
            true,
            cancellationToken);

        return Result<PaginatedListModel<ProductModel>>.Succeeded(new(productsDataGrid.Items.ConvertAll(p => p.ToProductModel()), productsDataGrid.TotalItems));
    }
}

