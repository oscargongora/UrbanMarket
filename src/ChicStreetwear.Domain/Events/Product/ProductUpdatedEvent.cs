using ChicStreetwear.Domain.Common;

namespace ChicStreetwear.Domain.Events.Product;
public sealed class ProductUpdatedEvent : DomainEventBase
{
    public required ProductAggregate.Product product { get; set; }
    public required ProductAggregate.Product updatedProduct { get; set; }
}
