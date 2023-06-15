using ChicStreetwear.Shared;

namespace ChicStreetwear.Domain.Errors;

public static class ProductReviewErrors
{
    public static Error InvalidRating => Error.BadRequest("ProductReview.InvalidRating", "The rating value must be between 1 and 5.");

    public static Error NotFound => Error.NotFound("ProductReview.NotFound", "Product review not found.");
}