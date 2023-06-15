using ChicStreetwear.Application.Common.Commands;
using static ChicStreetwear.Api.Requests;
using static ChicStreetwear.Server.Requests.Address;

namespace ChicStreetwear.Server.Requests;

public static partial class Requests
{
    public static class User
    {
        public sealed class CreateUserRequest
        {
            public required Guid applicationUserId { get; init; }
            public required string userName { get; init; }
        }

        #region address
        public abstract class UserAddressRequestBase
        {
            public required Guid userId { get; set; }
            public required string nameIdentifier { get; set; }
            public required AddressRequest address { get; set; }
        }
        public sealed class AddUserAddressRequest : UserAddressRequestBase { }
        public sealed class UpdateUserAddressRequest : UserAddressRequestBase
        {
            public required AddressRequest newAddress { get; set; }
        }
        public sealed class DeleteUserAddressRequest : UserAddressRequestBase { }
        #endregion

        public sealed class GetUsersRequest : GetRequestBase
        {
            public string? name { get; set; } = default;
            public string? address { get; set; } = default;
            public string? country { get; set; } = default;
            public string? city { get; set; } = default;
            public string? state { get; set; } = default;
            public string? postalCode { get; set; } = default;
        }
    }
}