using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Events.ProductReview;
using MediatR;

namespace ChicStreetwear.Application.ProductReviews.EventHandlers;
public sealed class ProductReviewCreatedEventHandler : INotificationHandler<ProductReviewCreatedEvent>
{
    private readonly IProductRepository _productRepository;

    public ProductReviewCreatedEventHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(ProductReviewCreatedEvent notification, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(notification.productId, cancellationToken);
        if (product is null) return;
        var result = product.AddRating(notification.rating);
        if(result.HasErrors) return;
        await _productRepository.UpdateAsync(result.Data, cancellationToken);
        return;
    }
}
