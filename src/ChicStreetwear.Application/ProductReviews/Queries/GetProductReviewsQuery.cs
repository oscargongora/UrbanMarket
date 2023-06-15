using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Application.Common.Mappers;
using ChicStreetwear.Application.Common.Queries;
using ChicStreetwear.Shared;
using MediatR;

namespace ChicStreetwear.Application.ProductReviews.Queries;
public sealed class GetProductReviewsQuery : GetQueryBase, IRequest<Result<IEnumerable<ProductReviewResponse>>>
{
    public Guid? ProductId { get; set; }
    public Guid? SellerId { get; set; }
    public Guid? CustomerId { get; set; }
}

public sealed class GetProductReviewsQueryHandler : IRequestHandler<GetProductReviewsQuery, Result<IEnumerable<ProductReviewResponse>>>
{
    private readonly IProductReviewRepository _productReviewRepository;

    public GetProductReviewsQueryHandler(IProductReviewRepository productReviewRepository)
    {
        _productReviewRepository = productReviewRepository;
    }

    async Task<Result<IEnumerable<ProductReviewResponse>>> IRequestHandler<GetProductReviewsQuery, Result<IEnumerable<ProductReviewResponse>>>.Handle(GetProductReviewsQuery request, CancellationToken cancellationToken)
    {
        var productReviews = await _productReviewRepository.ListAsync(request.ProductId, request.CustomerId, request.SellerId, request.Page, request.Take, true, cancellationToken);

        return Result<IEnumerable<ProductReviewResponse>>.Succeeded(productReviews.ConvertAll(pr => pr.ToProductReviewResponse()));
    }
}
