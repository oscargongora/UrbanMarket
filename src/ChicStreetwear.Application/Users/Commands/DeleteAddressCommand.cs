using ChicStreetwear.Application.Common.Commands;
using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Aggregates.User;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Shared;
using MediatR;

namespace ChicStreetwear.Application.Users.Commands;
public sealed class DeleteUserAddressCommand : IRequest<Result<Guid>>
{
    public required Guid userId { get; set; }
    public required string nameIdentifier { get; set; }
    public required AddressCommand address { get; set; }
}

public sealed class DeleteUserAddressCommandHandler : IRequestHandler<DeleteUserAddressCommand, Result<Guid>>
{
    private readonly IRepository<User> _userRepository;

    public DeleteUserAddressCommandHandler(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<Guid>> Handle(DeleteUserAddressCommand request, CancellationToken cancellationToken)
    {
        bool tryParse = Guid.TryParse(request.nameIdentifier, out Guid applicationUserId);
        if (!tryParse) return UserErrors.InvalidNameIdentifier;

        var user = await _userRepository.FirstOrDefaultAsync(u => u.Id.Equals(request.userId) && u.ApplicationUserId.Equals(applicationUserId));
        if (user is null) return UserErrors.NotFound;
        user.RemoveAddress(request.address.ToAddress);
        await _userRepository.UpdateAsync(user);
        return Result<Guid>.Succeeded(user.Id);
    }
}
