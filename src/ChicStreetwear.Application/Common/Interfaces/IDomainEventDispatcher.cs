using ChicStreetwear.Domain.Common;

namespace ChicStreetwear.Application.Common.Interfaces;

public interface IDomainEventDispatcher
{
    Task PublishDomainEvents(IEnumerable<EntityBase> entities);
}