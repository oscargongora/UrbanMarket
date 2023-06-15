using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Common.Interfaces;
using ChicStreetwear.Infrastructure.Identity;
using ChicStreetwear.Shared.Models.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq.Expressions;

namespace ChicStreetwear.Infrastructure.Persistence.Repositories;
public class Repository<T> : IRepository<T> where T : class, IAggregateRoot
{
    protected readonly ChicStreetwearIdentityDbContext _dbContext;
    protected DbSet<T> Set => _dbContext.Set<T>();
    protected DatabaseFacade Database => _dbContext.Database;

    public Repository(ChicStreetwearIdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Add(entity);
        await SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().AddRange(entities);
        await SaveChangesAsync(cancellationToken);
        return entities;
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().AnyAsync(predicate, cancellationToken);
    }

    public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().AnyAsync(cancellationToken);
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().CountAsync(predicate, cancellationToken);
    }

    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().CountAsync(cancellationToken);
    }

    public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Remove(entity);

        await SaveChangesAsync(cancellationToken);
    }

    public virtual async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().RemoveRange(entities);

        await SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        if (asNoTracking)
            return await _dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);
        return await _dbContext.Set<T>().FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public virtual async Task<T2?> FirstOrDefaultAsync<T2>(Expression<Func<T, bool>> predicate, Expression<Func<T, T2>> selector, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<T>().AsSplitQuery();
        query = asNoTracking ? query.AsNoTracking() : query;
        return await query.Where(predicate).Select(selector).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
    {
        return await _dbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken);
    }

    public virtual async Task<List<T>> ListAsync(bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<T>().AsQueryable();

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<List<T>> ListAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<T>().Where(predicate);

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<List<T>> ListAsync(List<Expression<Func<T, bool>>> predicates,
                                                 List<Expression<Func<T, object>>> includes,
                                                 bool asNoTracking = false,
                                                 CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<T>().AsQueryable();

        query = includes.Aggregate(query, (current, include) => current.Include(include));
        query = predicates.Aggregate(query, (current, predicate) => current.Where(predicate));

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<PaginatedListModel<T>> PaginatedListAsync(List<Expression<Func<T, bool>>> predicates,
                                                                List<Expression<Func<T, object>>> includes,
                                                                int page = 1,
                                                                int take = 10,
                                                                bool asNoTracking = false,
                                                                CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<T>().AsQueryable();

        query = includes.Aggregate(query, (current, include) => current.Include(include));
        query = predicates.Aggregate(query, (current, predicate) => current.Where(predicate));

        var totalItems = await query.CountAsync(cancellationToken);

        query = query.Skip((page - 1) * take).Take(take);

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return new(await query.ToListAsync(cancellationToken), totalItems);
    }

    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        if (asNoTracking)
            return await _dbContext.Set<T>().AsNoTracking().SingleOrDefaultAsync(predicate, cancellationToken);
        return await _dbContext.Set<T>().SingleOrDefaultAsync(predicate, cancellationToken);
    }

    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;

        await SaveChangesAsync(cancellationToken);
    }

    public virtual async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        await SaveChangesAsync(cancellationToken);
    }

    public IQueryable<T> AsQueryable() => _dbContext.Set<T>().AsQueryable();
}
