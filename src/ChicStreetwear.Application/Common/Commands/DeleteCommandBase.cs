using ChicStreetwear.Shared.Interfaces;

namespace ChicStreetwear.Application.Common.Commands;

public abstract class DeleteCommandBase : IEntityModelBase
{
    public required Guid Id { get; set; }
}