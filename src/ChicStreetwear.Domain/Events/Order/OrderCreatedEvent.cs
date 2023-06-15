using ChicStreetwear.Domain.Common;

namespace ChicStreetwear.Domain.Events.Order;
public sealed class OrderCreatedEvent : DomainEventBase
{
    public required Guid Id { get; set; }
}