using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Application.Common.Interfaces;
using ChicStreetwear.Domain.Aggregates.ProductReview;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Shared;
using MediatR;
using ChicStreetwear.Domain.Events.ProductReview;

namespace ChicStreetwear.Application.ProductReviews.Commands;
public sealed record UpdateProductReviewCommand(Guid id, int rating, string comment) : IRequest<Result<Guid>>;

public sealed class UpdateProductReviewCommandHandler : IRequestHandler<UpdateProductReviewCommand, Result<Guid>>
{
    private readonly IRepository<ProductReview> _productReviewRepository;
    private readonly IDateTimeService _timeService;

    public UpdateProductReviewCommandHandler(IRepository<ProductReview> productReviewRepository, IDateTimeService timeService)
    {
        _productReviewRepository = productReviewRepository;
        _timeService = timeService;
    }

    public async Task<Result<Guid>> Handle(UpdateProductReviewCommand request, CancellationToken cancellationToken)
    {
        var productReview = await _productReviewRepository.GetByIdAsync(request.id, cancellationToken);
        if (productReview is null)
        {
            return ProductReviewErrors.NotFound;
        }
        var oldRating = productReview.Rating;
        var productReviewResult = productReview.Update(request.rating, request.comment, _timeService.Now);

        if (productReviewResult.HasErrors)
            return productReviewResult.Errors;

        productReviewResult.Data.RegisterDomainEvent(new ProductReviewUpdatedEvent
        {
            oldRating = oldRating,
            newRating = productReviewResult.Data.Rating,
            productId = productReviewResult.Data.ProductId,
            sellerId = productReviewResult.Data.SellerId,
        });

        await _productReviewRepository.UpdateAsync(productReview);

        return Result<Guid>.Succeeded(productReviewResult.Data.Id);
    }
}
