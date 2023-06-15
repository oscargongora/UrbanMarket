using ChicStreetwear.Application.Common.Responses;
using ChicStreetwear.Application.Users.Queries;
using ChicStreetwear.Domain.Aggregates.User;
using ChicStreetwear.Domain.ValueObjects;
using Riok.Mapperly.Abstractions;

namespace ChicStreetwear.Application.Common.Mappers;

[Mapper]
internal static partial class UserMapper
{
    internal static partial UserResponse ToUserResponse(this User order);
    private static partial AddressResponse ToAddressResponse(this Address address);
}