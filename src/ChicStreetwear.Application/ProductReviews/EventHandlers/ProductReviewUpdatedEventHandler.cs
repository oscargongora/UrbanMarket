using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Events.ProductReview;
using MediatR;

namespace ChicStreetwear.Application.ProductReviews.EventHandlers;
public sealed class ProductReviewUpdatedEventHandler : INotificationHandler<ProductReviewUpdatedEvent>
{
    private readonly IProductRepository _productRepository;

    public ProductReviewUpdatedEventHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(ProductReviewUpdatedEvent notification, CancellationToken cancellationToken)
    {
        if (notification.oldRating != notification.newRating)
        {
            var product = await _productRepository.GetByIdAsync(notification.productId, cancellationToken);
            if (product is null) return;
            product.RemoveRating(notification.oldRating);
            product.AddRating(notification.newRating);
            await _productRepository.UpdateAsync(product, cancellationToken);
        }
    }
}
