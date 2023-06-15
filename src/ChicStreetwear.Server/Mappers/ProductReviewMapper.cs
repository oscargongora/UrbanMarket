using ChicStreetwear.Application.ProductReviews.Commands;
using ChicStreetwear.Application.ProductReviews.Queries;
using Riok.Mapperly.Abstractions;
using static ChicStreetwear.Server.Requests.Requests.ProductReview;

namespace ChicStreetwear.Server.Mappers;

[Mapper]
internal static partial class ProductReviewMapper
{
    internal static partial CreateProductReviewCommand ToCommand(this CreateProductReviewRequest request);
    internal static partial UpdateProductReviewCommand ToCommand(this UpdateProductReviewRequest request);
    internal static partial GetProductReviewsQuery ToQuery(this GetProductReviewsRequest request);
}
