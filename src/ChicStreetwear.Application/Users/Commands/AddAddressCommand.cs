using ChicStreetwear.Application.Common.Commands;
using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Aggregates.User;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Shared;
using MediatR;

namespace ChicStreetwear.Application.Users.Commands;
public sealed class AddUserAddressCommand : IRequest<Result<Guid>>
{
    public required Guid userId { get; set; }
    public required string nameIdentifier { get; set; }
    public required AddressCommand address { get; set; }
}

public sealed class AddUserAddressCommandHandler : IRequestHandler<AddUserAddressCommand, Result<Guid>>
{
    private readonly IRepository<User> _userRepository;

    public AddUserAddressCommandHandler(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<Guid>> Handle(AddUserAddressCommand request, CancellationToken cancellationToken)
    {
        bool tryParse = Guid.TryParse(request.nameIdentifier, out Guid applicationUserId);
        if (!tryParse) return UserErrors.InvalidNameIdentifier;

        var user = await _userRepository.FirstOrDefaultAsync(u => u.Id.Equals(request.userId) && u.ApplicationUserId.Equals(applicationUserId));

        if (user is null) return UserErrors.NotFound;

        user.AddAddress(request.address.ToAddress);

        await _userRepository.UpdateAsync(user);
        return Result<Guid>.Succeeded(user.Id);
    }
}
