using ChicStreetwear.Application.Common.Queries;
using ChicStreetwear.Domain.Aggregates.Product;
using ChicStreetwear.Shared.Models.Components;
using System.Linq.Expressions;

namespace ChicStreetwear.Application.Common.Interfaces.Repositories;
public interface IProductRepository : IRepository<Product>
{
    Task<Guid?> GetSellerIdAsync(Guid productId, CancellationToken cancellationToken = default);

    Task<IEnumerable<Guid>> GetCategoriesIdsFromProductCategoryAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Product>> ListFeaturedProducts(int take, CancellationToken cancellationToken = default);

    Task<PaginatedListModel<Product>> PaginatedListAsync(
        List<Expression<Func<Product, bool>>> predicates,
        List<Expression<Func<Product, object>>> includes,
        SortedDictionary<int, QuerySortProperty> sorts,
        List<QueryFilterProperty> filters,
        int page,
        int take,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default);
}
