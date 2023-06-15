using ChicStreetwear.Application.ProductReviews;
using ChicStreetwear.Domain.Aggregates.ProductReview;
using Riok.Mapperly.Abstractions;

namespace ChicStreetwear.Application.Common.Mappers;

[Mapper]
internal static partial class ProductReviewMapper
{
    internal static partial ProductReviewResponse ToProductReviewResponse(this ProductReview productReview);
}
