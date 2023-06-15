using static ChicStreetwear.Api.Requests;

namespace ChicStreetwear.Server.Requests;

public static partial class Requests
{
    public static class ProductReview
    {
        public abstract class ProductReviewCommandRequestBase
        {
            public required int rating { get; set; }
            public required string comment { get; set; }
        }

        public sealed class CreateProductReviewRequest : ProductReviewCommandRequestBase
        {
            public required Guid productId { get; set; }
            public Guid customerId { get; set; }
        }
        public sealed class UpdateProductReviewRequest : ProductReviewCommandRequestBase
        {
            public Guid id { get; set; }
        }
        public sealed class GetProductReviewsRequest : GetRequestBase
        {
            public Guid? ProductId { get; set; }
            public Guid? SellerId { get; set; }
            public Guid? CustomerId { get; set; }
        }
    }
}
