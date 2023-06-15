using ChicStreetwear.Application.Users.Commands;
using ChicStreetwear.Application.Users.Queries;
using Riok.Mapperly.Abstractions;
using static ChicStreetwear.Server.Requests.Requests.User;

namespace ChicStreetwear.Server.Mappers;

[Mapper]
internal static partial class UserMapper
{
    internal static partial CreateUserCommand ToCommand(this CreateUserRequest request);
    internal static partial AddUserAddressCommand ToCommand(this AddUserAddressRequest request);
    internal static partial UpdateUserAddressCommand ToCommand(this UpdateUserAddressRequest request);
    internal static partial DeleteUserAddressCommand ToCommand(this DeleteUserAddressRequest request);
    internal static partial GetUsersQuery ToQuery(this GetUsersRequest request);
}
