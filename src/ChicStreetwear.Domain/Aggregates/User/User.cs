using ChicStreetwear.Domain.Common;
using ChicStreetwear.Domain.Common.Interfaces;
using ChicStreetwear.Domain.ValueObjects;
using ChicStreetwear.Shared;

namespace ChicStreetwear.Domain.Aggregates.User;
public sealed class User : EntityBase, IAggregateRoot
{
    private readonly List<Address> _addresses = new();
    public Guid ApplicationUserId { get; }
    public string? UserName { get; }
    public IReadOnlyList<Address> Addresses => _addresses.AsReadOnly();

    private User(Guid id, Guid applicationUserId, string? userName) : base(id)
    {
        ApplicationUserId = applicationUserId;
        UserName = userName;
    }

    public static Result<User> New(Guid applicationUserId, string? userName = null)
    {
        var user = new User(Guid.NewGuid(), applicationUserId, userName);
        return Result<User>.Succeeded(user);
    }

    public Result<User> AddAddress(Address address)
    {
        _addresses.Add(address);
        return Result<User>.Succeeded(this);
    }

    public Result<User> UpdateAddress(Address oldAddress, Address newAddress)
    {
        _addresses.Remove(oldAddress);
        _addresses.Add(newAddress);
        return Result<User>.Succeeded(this);
    }

    public Result<User> RemoveAddress(Address address)
    {
        _addresses.Remove(address);
        return Result<User>.Succeeded(this);
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private User() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
