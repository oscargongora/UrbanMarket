using ChicStreetwear.Shared.Interfaces;

namespace ChicStreetwear.Application.Common.Queries;
public abstract class GetByIdQueryBase : IEntityModelBase
{
    public required Guid Id { get; set; }
}
