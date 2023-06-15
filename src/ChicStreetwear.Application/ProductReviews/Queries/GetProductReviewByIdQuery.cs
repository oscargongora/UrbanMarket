using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Application.Common.Mappers;
using ChicStreetwear.Application.Common.Queries;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Shared;
using MediatR;

namespace ChicStreetwear.Application.ProductReviews.Queries;
public sealed class GetProductReviewByIdQuery : GetByIdQueryBase, IRequest<Result<ProductReviewResponse>> { };

public sealed class GetProductReviewByIdQueryHandler : IRequestHandler<GetProductReviewByIdQuery, Result<ProductReviewResponse>>
{
    private readonly IProductReviewRepository _productReviewRepository;

    public GetProductReviewByIdQueryHandler(IProductReviewRepository productReviewRepository)
    {
        _productReviewRepository = productReviewRepository;
    }

    public async Task<Result<ProductReviewResponse>> Handle(GetProductReviewByIdQuery request, CancellationToken cancellationToken)
    {
        var productReview = await _productReviewRepository.GetByIdAsync(request.Id, cancellationToken);
        if (productReview is null)
        {
            return ProductReviewErrors.NotFound;
        }
        return Result<ProductReviewResponse>.Succeeded(productReview.ToProductReviewResponse());
    }
}
