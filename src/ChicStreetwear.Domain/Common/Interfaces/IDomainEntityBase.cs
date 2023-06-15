using ChicStreetwear.Shared.Interfaces;

namespace ChicStreetwear.Domain.Common.Interfaces;
public interface IDomainEntityBase
{
    IReadOnlyList<DomainEventBase> DomainEvents { get; }
    void RegisterDomainEvent(DomainEventBase domainEvent);
    void ClearDomainEvents();
}
