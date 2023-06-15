using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Aggregates.User;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Shared;
using MediatR;

namespace ChicStreetwear.Application.Users.Queries;
public sealed class GetUserIdByNameIdentifierQuery : IRequest<Result<Guid?>>
{
    public required string nameIdentifier { get; set; }
}

public sealed class GetUserIdByNameIdentifierQueryHandler : IRequestHandler<GetUserIdByNameIdentifierQuery, Result<Guid?>>
{
    private readonly IRepository<User> _userRepository;

    public GetUserIdByNameIdentifierQueryHandler(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<Guid?>> Handle(GetUserIdByNameIdentifierQuery request, CancellationToken cancellationToken)
    {
        bool tryParse = Guid.TryParse(request.nameIdentifier, out Guid applicationUserId);
        if (!tryParse) return UserErrors.InvalidNameIdentifier;
        var userId = await _userRepository.FirstOrDefaultAsync<Guid?>(u => u.ApplicationUserId.Equals(applicationUserId), u => u.Id, true, cancellationToken);
        return Result<Guid?>.Succeeded(userId);
    }
}
