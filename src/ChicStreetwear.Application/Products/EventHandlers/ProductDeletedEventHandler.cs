using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Aggregates.Cart;
using ChicStreetwear.Domain.Events.Product;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChicStreetwear.Application.Products.EventHandlers;
public sealed class ProductDeletedEventHandler : INotificationHandler<ProductDeletedEvent>
{
    private readonly ILogger<ProductDeletedEventHandler> _logger;
    private readonly IRepository<Cart> _cartRepository;

    public ProductDeletedEventHandler(IRepository<Cart> cartRepository, ILogger<ProductDeletedEventHandler> logger)
    {
        _cartRepository = cartRepository;
        _logger = logger;
    }

    public async Task Handle(ProductDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogWarning("Handling product deleted.");
        //await _cartRepository.RemoveCartProductsByProductIdColummn(notification.id);
    }
}
