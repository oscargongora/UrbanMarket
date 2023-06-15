using ChicStreetwear.Domain.Aggregates.ProductReview;

namespace ChicStreetwear.Application.Common.Interfaces.Repositories;
public interface IProductReviewRepository : IRepository<ProductReview>
{
    Task<List<ProductReview>> ListAsync(Guid? productId, Guid? sellerId, Guid? customerId, int? page = null, int? take = null, bool asNoTracking = false, CancellationToken cancellationToken = default);
}
