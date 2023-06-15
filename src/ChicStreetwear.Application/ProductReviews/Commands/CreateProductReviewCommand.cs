using ChicStreetwear.Application.Common.Interfaces;
using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Aggregates.ProductReview;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Domain.Events.ProductReview;
using ChicStreetwear.Shared;
using MediatR;

namespace ChicStreetwear.Application.ProductReviews.Commands;
public sealed record CreateProductReviewCommand(int rating, string comment, Guid customerId, Guid productId) : IRequest<Result<Guid>>;

public sealed class CreateProductReviewCommandHandler : IRequestHandler<CreateProductReviewCommand, Result<Guid>>
{
    private readonly IProductRepository _productRepository;
    private readonly IDateTimeService _timeService;
    private readonly IProductReviewRepository _productReviewRepository;

    public CreateProductReviewCommandHandler(IProductRepository productRepository, IDateTimeService timeService, IProductReviewRepository productReviewRepository)
    {
        _productRepository = productRepository;
        _timeService = timeService;
        _productReviewRepository = productReviewRepository;
    }

    public async Task<Result<Guid>> Handle(CreateProductReviewCommand request, CancellationToken cancellationToken)
    {
        var sellerId = await _productRepository.GetSellerIdAsync(request.productId, cancellationToken);
        if (sellerId is null)
        {
            return ProductErrors.ProductNotFound;
        }
        var productReviewResult = ProductReview.New(request.rating, request.comment, (Guid)sellerId, request.customerId, request.productId, _timeService.Now, _timeService.Now);

        if (productReviewResult.HasErrors)
            return productReviewResult.Errors;

        productReviewResult.Data.RegisterDomainEvent(new ProductReviewCreatedEvent
        {
            rating = productReviewResult.Data.Rating,
            productId = productReviewResult.Data.ProductId,
            sellerId = productReviewResult.Data.SellerId
        });

        await _productReviewRepository.AddAsync(productReviewResult.Data, cancellationToken);

        return Result<Guid>.Succeeded(productReviewResult.Data.Id);
    }
}
