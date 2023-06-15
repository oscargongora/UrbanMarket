using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Aggregates.ProductReview;
using ChicStreetwear.Infrastructure.Extensions;
using ChicStreetwear.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChicStreetwear.Infrastructure.Persistence.Repositories;
public sealed class ProductReviewRepository : Repository<ProductReview>, IProductReviewRepository
{
    public ProductReviewRepository(ChicStreetwearIdentityDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<ProductReview>> ListAsync(Guid? productId, Guid? sellerId, Guid? customerId, int? page = null, int? take = null, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        var productReviewsQuery = _dbContext.Set<ProductReview>()
            .WhereIf(productId is not null, pr => pr.ProductId.Equals(productId))
            .WhereIf(sellerId is not null, pr => pr.SellerId.Equals(sellerId))
            .WhereIf(customerId is not null, pr => pr.CustomerId.Equals(customerId));

        if (page is not null && take is not null)
        {
            productReviewsQuery = productReviewsQuery.Skip(((int)page - 1) * (int)take).Take((int)take);
        }
        return asNoTracking ? await productReviewsQuery.AsNoTracking().ToListAsync(cancellationToken) : await productReviewsQuery.ToListAsync(cancellationToken);
    }
}
