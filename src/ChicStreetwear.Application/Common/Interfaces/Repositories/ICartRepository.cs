using ChicStreetwear.Domain.Aggregates.Cart;

namespace ChicStreetwear.Application.Common.Interfaces.Repositories;
public interface ICartRepository : IRepository<Cart>
{
    Task RemoveCartProductsByProductIdColummn(Guid productId);
}
