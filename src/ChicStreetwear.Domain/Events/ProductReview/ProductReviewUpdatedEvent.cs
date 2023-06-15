using ChicStreetwear.Domain.Common;

namespace ChicStreetwear.Domain.Events.ProductReview;
public sealed class ProductReviewUpdatedEvent : DomainEventBase
{
    public required int oldRating { get; set; }
    public required int newRating { get; set; }
    public required Guid productId { get; set; }
    public required Guid sellerId { get; set; }
}