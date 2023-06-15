using ChicStreetwear.Domain.Common;
using ChicStreetwear.Domain.Common.Interfaces;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Shared;

namespace ChicStreetwear.Domain.Aggregates.ProductReview;
public sealed class ProductReview : EntityBase, IAggregateRoot
{
    public int Rating { get; private set; }
    public string Comment { get; private set; }
    public Guid SellerId { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid ProductId { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }

    private ProductReview(Guid id, int rating, string comment, Guid sellerId, Guid customerId, Guid productId, DateTime createdDateTime, DateTime updatedDateTime) : base(id)
    {
        Rating = rating;
        Comment = comment;
        SellerId = sellerId;
        CustomerId = customerId;
        ProductId = productId;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    public static Result<ProductReview> New(int rating, string comment, Guid sellerId, Guid customerId, Guid productId, DateTime createdDateTime, DateTime updatedDateTime)
    {
        if (rating < 1 || rating > 5) return ProductReviewErrors.InvalidRating;
        ProductReview productReview = new(Guid.NewGuid(), rating, comment, sellerId, customerId, productId, createdDateTime, updatedDateTime);
        return Result<ProductReview>.Succeeded(productReview);
    }

    public Result<ProductReview> Update(int rating, string comment, DateTime updatedDateTime)
    {
        if (rating < 1 || rating > 5) return ProductReviewErrors.InvalidRating;
        Rating = rating;
        Comment = comment;
        UpdatedDateTime = updatedDateTime;
        return Result<ProductReview>.Succeeded(this);
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ProductReview() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
