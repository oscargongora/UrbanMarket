namespace ChicStreetwear.Application.ProductReviews;

public sealed record ProductReviewResponse(Guid Id, int Rating, string Comment, Guid SellerId, Guid CustomerId, Guid ProductId, DateTime CreatedDateTime, DateTime UpdatedDateTime);