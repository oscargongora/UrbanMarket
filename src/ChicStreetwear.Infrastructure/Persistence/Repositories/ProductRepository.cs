using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Application.Common.Queries;
using ChicStreetwear.Domain.Aggregates.Product;
using ChicStreetwear.Domain.Aggregates.Product.Enums;
using ChicStreetwear.Infrastructure.Identity;
using ChicStreetwear.Shared.Models.Components;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChicStreetwear.Infrastructure.Persistence.Repositories;
public sealed class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ChicStreetwearIdentityDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Guid>> GetCategoriesIdsFromProductCategoryAsync(CancellationToken cancellationToken = default)
    {
        var query = """
            SELECT productCategory.CategoryId FROM ProductCategory productCategory
            """;
        var categoriesIds = _dbContext.Database.SqlQueryRaw<Guid>(query);
        return await categoriesIds.ToListAsync();
    }

    public async Task<Guid?> GetSellerIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var product = await _dbContext.Set<Product>()
                                      .AsNoTracking()
                                      .AsSplitQuery()
                                      .Select(p => new { p.Id, p.SellerId })
                                      .FirstOrDefaultAsync(p => p.Id.Equals(productId), cancellationToken);
        return product?.SellerId;
    }

    public async Task<IEnumerable<Product>> ListFeaturedProducts(int take, CancellationToken cancellationToken = default)
    {
        var products = await _dbContext.Set<Product>()
                                       .AsNoTracking()
                                       .Where(p => !p.Variations.Any())
                                       .Where(p => p.Stock.Quantity > 0)
                                       .OrderByDescending(p => p.Rating.Value)
                                       .Take(take)
                                       .ToListAsync();
        return products;
    }


    private Expression<Func<Product, object>>? GetPropertySelector(string propertyName)
    {
        Expression<Func<Product, object>>? property = null;
        if (propertyName.Equals(nameof(Product.Name)))
        {
            property = o => o.Name;
        }
        else if (propertyName.Equals(nameof(Product.Description)))
        {
            property = o => o.Description;
        }
        else if (propertyName.Equals(nameof(Product.Stock)))
        {
            property = o => o.Stock.Quantity;
        }
        else if (propertyName.Equals(nameof(Product.Rating)))
        {
            property = o => o.Rating.Value;
        }
        else if (propertyName.Equals(nameof(Product.Status)))
        {
            property = o => o.Status;
        }
        return property;
    }

    private Expression<Func<Product, bool>>? GetPropertyPredicate(string name, string value, string _operator)
    {
        Expression<Func<Product, bool>>? property = null;
        if (name.Equals(nameof(Product.Status)))
        {
            if (_operator.Equals("is"))
            {
                return p => p.Status.Equals(Enum.Parse<ProductStatus>(value));
            }
            else
            {
                return p => !p.Status.Equals(Enum.Parse<ProductStatus>(value));
            }
        }
        return property;
    }

    public async Task<PaginatedListModel<Product>> PaginatedListAsync(
        List<Expression<Func<Product, bool>>> predicates,
        List<Expression<Func<Product, object>>> includes,
        SortedDictionary<int, QuerySortProperty> sorts,
        List<QueryFilterProperty> filters,
        int page,
        int take,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default)
    {
        List<Product> orders;

        var query = _dbContext.Products.AsQueryable();

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        query = includes.Aggregate(query, (current, include) => current.Include(include));
        query = predicates.Aggregate(query, (current, predicate) => current.Where(predicate));

        foreach (var filter in filters)
        {
            var predicate = GetPropertyPredicate(filter.Name, filter.Value, filter.Operator);
            if (predicate is not null)
            {
                query = query.Where(predicate);
            }
        }

        var totalItems = await query.CountAsync(cancellationToken);

        if (sorts.Any())
        {
            var firstPropertyIndex = 0;
            Expression<Func<Product, object>>? property = null;
            IOrderedQueryable<Product>? orderQuery = null;
            foreach (var sort in sorts)
            {
                property = GetPropertySelector(sort.Value.Name);
                if (property is not null)
                {
                    orderQuery = sort.Value.Order.Equals("asc") ?
                        query.OrderBy(property) :
                        query.OrderByDescending(property);
                    break;
                }
                firstPropertyIndex++;
            }
            if (orderQuery is not null)
            {
                var index = 0;
                foreach (var sort in sorts)
                {
                    if (index > firstPropertyIndex)
                    {
                        property = GetPropertySelector(sort.Value.Name);
                        if (property is not null)
                        {
                            orderQuery = sort.Value.Order.Equals("asc") ?
                                orderQuery.ThenBy(property) :
                                orderQuery.ThenByDescending(property);
                            break;
                        }
                    }
                    index++;
                }

                query = query.Skip((page - 1) * take).Take(take);
                orders = await orderQuery.ToListAsync(cancellationToken);
                return new PaginatedListModel<Product>(orders, totalItems);
            }
        }

        query = query.Skip((page - 1) * take).Take(take);
        orders = await query.ToListAsync(cancellationToken);
        return new PaginatedListModel<Product>(orders, totalItems);
    }
}
