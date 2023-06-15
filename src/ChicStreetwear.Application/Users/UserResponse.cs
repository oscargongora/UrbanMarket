using ChicStreetwear.Application.Common.Responses;


namespace ChicStreetwear.Application.Users.Queries;

public sealed class UserResponse : IEntityResponseBase
{
    public required Guid Id { get; set; }
    public required Guid ApplicationUserId { get; set; }
    public required string UserName { get; init; }
    public required List<AddressResponse> Addresses { get; init; }
}