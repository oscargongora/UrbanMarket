using ChicStreetwear.Application.Common.Interfaces;
using ChicStreetwear.Domain.Common;
using MediatR;

namespace ChicStreetwear.Infrastructure.Services;
public sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IPublisher _eventPublisher;

    public DomainEventDispatcher(IPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public async Task PublishDomainEvents(IEnumerable<EntityBase> entities)
    {
        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        entities.ToList().ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _eventPublisher.Publish(domainEvent).ConfigureAwait(false);
        }
    }
}
