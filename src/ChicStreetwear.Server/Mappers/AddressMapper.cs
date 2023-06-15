using ChicStreetwear.Application.Common.Commands;
using Riok.Mapperly.Abstractions;
using static ChicStreetwear.Server.Requests.Address;

namespace ChicStreetwear.Server.Mappers;

[Mapper]
public static partial class AddressMapper
{
    public static partial AddressCommand ToCommand(this AddressRequest address);
}
