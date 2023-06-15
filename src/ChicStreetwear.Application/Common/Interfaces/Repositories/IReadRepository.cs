using ChicStreetwear.Domain.Common.Interfaces;
using ChicStreetwear.Shared.Models.Components;
using System.Linq.Expressions;

namespace ChicStreetwear.Application.Common.Interfaces.Repositories;
public interface IReadRepository<T> where T : class, IAggregateRoot
{
    Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull;

    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = false, CancellationToken cancellationToken = default);

    Task<T2?> FirstOrDefaultAsync<T2>(Expression<Func<T, bool>> predicate, Expression<Func<T, T2>> selector, bool asNoTracking = false, CancellationToken cancellationToken = default);

    Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = false, CancellationToken cancellationToken = default);

    Task<List<T>> ListAsync(bool asNoTracking = false, CancellationToken cancellationToken = default);

    Task<List<T>> ListAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = false, CancellationToken cancellationToken = default);

    Task<List<T>> ListAsync(List<Expression<Func<T, bool>>> predicates,
                            List<Expression<Func<T, object>>> includes,
                            bool asNoTracking = false,
                            CancellationToken cancellationToken = default);

    Task<PaginatedListModel<T>> PaginatedListAsync(List<Expression<Func<T, bool>>> predicates,
                                                 List<Expression<Func<T, object>>> includes,
                                                 int page = 1,
                                                 int take = 10,
                                                 bool asNoTracking = false,
                                                 CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(CancellationToken cancellationToken = default);

    IQueryable<T> AsQueryable();
}
