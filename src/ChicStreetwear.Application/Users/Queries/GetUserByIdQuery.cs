using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Application.Common.Mappers;
using ChicStreetwear.Application.Common.Queries;
using ChicStreetwear.Domain.Aggregates.User;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Shared;
using MediatR;

namespace ChicStreetwear.Application.Users.Queries;
public sealed class GetUserByIdQuery : GetByIdQueryBase, IRequest<Result<UserResponse>> { }

public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserResponse>>
{
    private readonly IRepository<User> _userRepository;

    public GetUserByIdQueryHandler(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user is null) return UserErrors.NotFound;
        return Result<UserResponse>.Succeeded(user.ToUserResponse());
    }
}