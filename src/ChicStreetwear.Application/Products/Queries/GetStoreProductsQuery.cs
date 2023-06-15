using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Application.Mappers;
using ChicStreetwear.Domain.Aggregates.Product;
using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Enums;
using ChicStreetwear.Shared.Models.Product;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChicStreetwear.Application.Products.Queries;
public sealed class GetStoreProductsQuery : IRequest<Result<IEnumerable<StoreProductModel>>>
{
    public Guid? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public float? MinRating { get; set; }
    public ProductSortOption? SortOption { get; set; }
    public int Page { get; set; } = 1;
    public int Take { get; set; } = 8;
}

public sealed class GetStoreProductsQueryHandler : IRequestHandler<GetStoreProductsQuery, Result<IEnumerable<StoreProductModel>>>
{
    private readonly IProductRepository _productRepository;

    public GetStoreProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<IEnumerable<StoreProductModel>>> Handle(GetStoreProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _productRepository.AsQueryable().AsNoTracking().Where(p => p.HasAttributes ? p.Variations.Any(v => v.Stock.Quantity > 0) : p.Stock.Quantity > 0);
        query.Where(p => !p.Variations.Any());

        if (request.CategoryId is not null)
        {
            query = query.Where(p => p.Categories.Any(pc => pc.CategoryId.Equals(request.CategoryId)));
        }
        if (request.MinPrice is not null)
        {
            query = query.Where(p => p.HasAttributes ? p.Variations.FirstOrDefault(v => v.Stock.Quantity > 0)!.RegularPrice.Amount >= request.MinPrice : p.RegularPrice!.Amount >= request.MinPrice);
        }
        if (request.MaxPrice is not null)
        {
            query = query.Where(p => p.HasAttributes ? p.Variations.FirstOrDefault(v => v.Stock.Quantity > 0)!.RegularPrice.Amount <= request.MaxPrice : p.RegularPrice!.Amount <= request.MaxPrice);
        }
        if (request.MinRating is not null)
        {
            query = query.Where(p => p.Rating.Value >= request.MinRating);
        }

        IOrderedQueryable<Product>? orderQuery = null;

        if (request.SortOption.Equals(ProductSortOption.Newest))
        {
            orderQuery = query.OrderByDescending(p => p.CreatedDate);
        }
        else if (request.SortOption.Equals(ProductSortOption.BestSeller))
        {
            orderQuery = query.OrderByDescending(p => p.SalesAmount);
        }
        else if (request.SortOption.Equals(ProductSortOption.LowerPrice))
        {
            orderQuery = query.OrderBy(p => p.HasAttributes ? (p.Variations.FirstOrDefault(v => v.Stock.Quantity > 0)!.RegularPrice.Amount) : p.RegularPrice!.Amount);
        }
        else if (request.SortOption.Equals(ProductSortOption.HigherPrice))
        {
            orderQuery = query.OrderByDescending(p => p.HasAttributes ? (p.Variations.FirstOrDefault(v => v.Stock.Quantity > 0)!.RegularPrice.Amount) : p.RegularPrice!.Amount);
        }
        else if (request.SortOption.Equals(ProductSortOption.HighestRating))
        {
            orderQuery = query.OrderByDescending(p => p.Rating.Value);
        }

        List<Product> products;
        if (orderQuery is not null)
        {
            products = await orderQuery
                .Skip((request.Page - 1) * request.Take)
                .Take(request.Take)
                .ToListAsync(cancellationToken);
        }
        else
        {
            products = await query
                .Skip((request.Page - 1) * request.Take)
                .Take(request.Take)
                .ToListAsync(cancellationToken);
        }
        return Result<IEnumerable<StoreProductModel>>.Succeeded(products.Select(p => p.ToStoreProductModel()));

    }
}
