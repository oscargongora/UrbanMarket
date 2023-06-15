using ChicStreetwear.Domain.Common;

namespace ChicStreetwear.Domain.Events.Product;
public sealed class ProductDeletedEvent : DomainEventBase
{
    public required Guid id { get; set; }
}