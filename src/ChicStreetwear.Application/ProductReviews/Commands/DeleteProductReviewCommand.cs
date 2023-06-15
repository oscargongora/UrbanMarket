using ChicStreetwear.Application.Common.Interfaces;
using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Aggregates.ProductReview;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Domain.Events.ProductReview;
using ChicStreetwear.Shared;
using MediatR;

namespace ChicStreetwear.Application.ProductReviews.Commands;
public sealed record DeleteProductReviewCommand(Guid id) : IRequest<Result<Guid>>;

public sealed class DeleteProductReviewCommandHandler : IRequestHandler<DeleteProductReviewCommand, Result<Guid>>
{
    private readonly IRepository<ProductReview> _productReviewRepository;
    private readonly IDateTimeService _timeService;

    public DeleteProductReviewCommandHandler(IRepository<ProductReview> productReviewRepository, IDateTimeService timeService)
    {
        _productReviewRepository = productReviewRepository;
        _timeService = timeService;
    }

    public async Task<Result<Guid>> Handle(DeleteProductReviewCommand request, CancellationToken cancellationToken)
    {
        var productReview = await _productReviewRepository.GetByIdAsync(request.id, cancellationToken);
        if (productReview is null)
        {
            return ProductReviewErrors.NotFound;
        }
        productReview.RegisterDomainEvent(new ProductReviewDeletedEvent
        {
            productId = productReview.ProductId,
            rating = productReview.Rating,
            sellerId = productReview.SellerId
        });

        await _productReviewRepository.DeleteAsync(productReview);

        return Result<Guid>.Succeeded(request.id);
    }
}
