using ChicStreetwear.Domain.Common;

namespace ChicStreetwear.Domain.Events.ProductReview;
public sealed class ProductReviewDeletedEvent : DomainEventBase
{
    public required int rating { get; set; }
    public required Guid productId { get; set; }
    public required Guid sellerId { get; set; }
}