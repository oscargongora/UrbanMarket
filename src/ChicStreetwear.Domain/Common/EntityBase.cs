using ChicStreetwear.Domain.Common.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChicStreetwear.Domain.Common;

public abstract class EntityBase : IDomainEntityBase
{
    public Guid Id { get; protected set; }

    #region domain events
    private List<DomainEventBase> _domainEvents = new();
    [NotMapped]
    public IReadOnlyList<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

    public void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
    #endregion

    protected EntityBase(Guid id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        return obj is EntityBase entity && Id.Equals(entity.Id);
    }

    public static bool operator ==(EntityBase e1, EntityBase e2)
    {
        return Equals(e1, e2);
    }

    public static bool operator !=(EntityBase e1, EntityBase e2)
    {
        return !Equals(e1, e2);
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public bool Equals(EntityBase? other)
    {
        return Equals((object?)other);
    }

#pragma warning disable CS8618
    protected EntityBase()
    {
    }
#pragma warning restore CS8618
}