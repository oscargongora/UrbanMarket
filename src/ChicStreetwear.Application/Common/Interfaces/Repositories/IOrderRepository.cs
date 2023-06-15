using ChicStreetwear.Application.Common.Queries;
using ChicStreetwear.Domain.Aggregates.Cart;
using ChicStreetwear.Domain.Aggregates.Order;
using ChicStreetwear.Shared.Models.Components;
using System.Linq.Expressions;

namespace ChicStreetwear.Application.Common.Interfaces.Repositories;
public interface IOrderRepository : IRepository<Order>
{
    Task<bool> CreateOrderAndRemoveCart(Order order, Cart cart, CancellationToken cancellationToken = default);
    Task<PaginatedListModel<Order>> PaginatedListAsync(
        List<Expression<Func<Order, bool>>> predicates,
        List<Expression<Func<Order, object>>> includes,
        SortedDictionary<int, QuerySortProperty> sorts,
        List<QueryFilterProperty> filters,
        int page = 1,
        int take = 10,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default);
}
