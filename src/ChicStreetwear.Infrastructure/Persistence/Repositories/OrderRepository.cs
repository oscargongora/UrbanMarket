using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Application.Common.Queries;
using ChicStreetwear.Domain.Aggregates.Cart;
using ChicStreetwear.Domain.Aggregates.Order;
using ChicStreetwear.Domain.Aggregates.Order.Enums;
using ChicStreetwear.Infrastructure.Identity;
using ChicStreetwear.Shared.Models.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace ChicStreetwear.Infrastructure.Persistence.Repositories;
public sealed class OrderRepository : Repository<Order>, IOrderRepository
{
    private readonly ILogger<OrderRepository> _logger;
    public OrderRepository(ChicStreetwearIdentityDbContext dbContext, ILogger<OrderRepository> logger) : base(dbContext)
    {
        _logger = logger;
    }

    private Expression<Func<Order, object>>? GetPropertySelector(string propertyName)
    {
        Expression<Func<Order, object>>? property = null;
        if (propertyName.Equals(nameof(Order.Status)))
        {
            property = o => o.Status;
        }
        else if (propertyName.Equals(nameof(Order.PlacedDate)))
        {
            property = o => o.PlacedDate;
        }
        else if (propertyName.Equals(nameof(Order.DispatchedDate)))
        {
            property = o => o.DispatchedDate;
        }
        else if (propertyName.Equals(nameof(Order.DeliveredDate)))
        {
            property = o => o.DeliveredDate;
        }
        else if (propertyName.Equals(nameof(Order.Total)))
        {
            property = o => o.Total;
        }
        return property;
    }

    private Expression<Func<Order, bool>>? GetPropertyPredicate(string name, string value, string _operator)
    {
        Expression<Func<Order, bool>>? property = null;
        if (name.Equals(nameof(Order.Status)))
        {
            if (_operator.Equals("is"))
            {
                return o => o.Status.Equals(Enum.Parse<OrderStatus>(value));
            }
            else
            {
                return o => !o.Status.Equals(Enum.Parse<OrderStatus>(value));
            }
        }
        return property;
    }

    public async Task<bool> CreateOrderAndRemoveCart(Order order, Cart cart, CancellationToken cancellationToken = default)
    {
        using var transaction = Database.BeginTransaction();
        try
        {
            await _dbContext.Set<Order>().AddAsync(order);
            _dbContext.Set<Cart>().Remove(cart);
            await _dbContext.SaveChangesAsync();
            transaction.Commit();
            return true;
        }
        catch (Exception exception)
        {
            transaction.Rollback();
            _logger.LogError(exception, exception.Message);
        }
        return false;
    }

    public async Task<PaginatedListModel<Order>> PaginatedListAsync(
        List<Expression<Func<Order, bool>>> predicates,
        List<Expression<Func<Order, object>>> includes,
        SortedDictionary<int, QuerySortProperty> sorts,
        List<QueryFilterProperty> filters,
        int page,
        int take,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default)
    {
        List<Order> orders;

        var query = _dbContext.Orders.AsQueryable();

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
            Expression<Func<Order, object>>? property = null;
            IOrderedQueryable<Order>? orderQuery = null;
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
                return new PaginatedListModel<Order>(orders, totalItems);
            }
        }

        query = query.Skip((page - 1) * take).Take(take);
        orders = await query.ToListAsync(cancellationToken);
        return new PaginatedListModel<Order>(orders, totalItems);
    }
}
