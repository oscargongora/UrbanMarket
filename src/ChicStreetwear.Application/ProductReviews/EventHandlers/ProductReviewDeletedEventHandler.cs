using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Events.ProductReview;
using MediatR;

namespace ChicStreetwear.Application.ProductReviews.EventHandlers;
public sealed class ProductReviewDeletedEventHandler : INotificationHandler<ProductReviewDeletedEvent>
{
    private readonly IProductRepository _productRepository;

    public ProductReviewDeletedEventHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(ProductReviewDeletedEvent notification, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(notification.productId, cancellationToken);
        if (product is null) return;
        product.RemoveRating(notification.rating);
        await _productRepository.UpdateAsync(product, cancellationToken);
    }
}
