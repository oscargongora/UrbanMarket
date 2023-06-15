using MediatR;

namespace ChicStreetwear.Domain.Common;

public abstract class DomainEventBase : INotification
{
    private readonly DateTime _creationDate = DateTime.UtcNow;
    public DateTime CreationDate => _creationDate;
}