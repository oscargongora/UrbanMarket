using ChicStreetwear.Application.Common.Queries;
using ChicStreetwear.Domain.Aggregates.Category;

using ChicStreetwear.Shared.Models.Components;

namespace ChicStreetwear.Application.Common.Interfaces.Repositories;
public interface ICategoryRepository : IRepository<Category>
{
    Task<PaginatedListModel<Category>> ListAsync(SortedDictionary<int, QuerySortProperty> sorts, string? search = null, int? page = null, int? take = null, CancellationToken cancellationToken = default);
}
