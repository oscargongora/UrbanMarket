using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Aggregates.Cart;
using ChicStreetwear.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChicStreetwear.Infrastructure.Persistence.Repositories;
public sealed class CartRepository : Repository<Cart>, ICartRepository
{
    private readonly ILogger<CartRepository> _logger;
    public CartRepository(ChicStreetwearIdentityDbContext dbContext, ILogger<CartRepository> logger) : base(dbContext)
    {
        _logger = logger;
    }

    public async Task RemoveCartProductsByProductIdColummn(Guid productId)
    {
        using var transaction = Database.BeginTransaction();
        try
        {
            await Database.ExecuteSqlAsync($"""
                UPDATE c
                SET c.Total = cp.sumPrice
                FROM Carts AS c
                INNER JOIN
                (
                    SELECT CartId, SUM(Quantity*Price) sumPrice
                    FROM CartProduct
                    WHERE ProductId={productId}
                    GROUP BY CartId 
                ) cp
                ON cp.CartId = c.Id
                """);

            await Database.ExecuteSqlAsync($"""
                DELETE FROM [dbo].[CartProduct]
                WHERE ProductId={productId}
                """);
            transaction.Commit();
        }
        catch (Exception exception)
        {
            transaction.Rollback();
            _logger.LogError(exception, exception.Message);
        }
    }
}
