using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Aggregates.User;
using ChicStreetwear.Shared;
using MediatR;

namespace ChicStreetwear.Application.Users.Commands;
public sealed class CreateUserCommand : IRequest<Result<Guid>>
{
    public required Guid applicationUserId { get; init; }
    public required string userName { get; init; }
}

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    private readonly IRepository<User> _userRepository;

    public CreateUserCommandHandler(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var userResult = User.New(request.applicationUserId, request.userName);
        if (userResult.HasErrors) return userResult.Errors;
        await _userRepository.AddAsync(userResult.Data);
        return Result<Guid>.Succeeded(userResult.Data.Id);
    }
}
