using ChicStreetwear.Application.Users.Queries;
using ChicStreetwear.Server.Mappers;
using ChicStreetwear.Shared.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ChicStreetwear.Server.Requests.Address;
using static ChicStreetwear.Server.Requests.Requests.User;

namespace ChicStreetwear.Server.Controllers;

public class UsersController : ApiControllerBase
{
    [HttpGet]
    [Authorize(Roles = IdentityDefaults.ADMINISTRATOR_ROLE)]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersRequest request)
    {
        var query = request.ToQuery();

        var getUsersQueryResult = await Sender.Send(query);
        return getUsersQueryResult.HasErrors ?
            Problem(getUsersQueryResult.Errors) :
            Ok(getUsersQueryResult.Data);
    }

    //[Route("GetUserByNameIdentifier")]
    [HttpGet("GetUserByNameIdentifier", Name = nameof(GetUserByNameIdentifier))]
    [Authorize]
    public async Task<IActionResult> GetUserByNameIdentifier()
    {
        var getUsersQueryResult = await Sender.Send(new GetUserIdByNameIdentifierQuery { nameIdentifier = UserNameIdentifier! });
        return getUsersQueryResult.HasErrors ?
            Problem(getUsersQueryResult.Errors) :
            Ok(getUsersQueryResult.Data);
    }

    [HttpGet("{id:guid}", Name = nameof(GetUserById))]
    [Authorize(Roles = IdentityDefaults.ADMINISTRATOR_ROLE)]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var getUserByIdResult = await Sender.Send(new GetUserByIdQuery { Id = id });
        return getUserByIdResult.HasErrors ?
            Problem(getUserByIdResult.Errors) :
            Ok(getUserByIdResult.Data);
    }

    [HttpPost("{userId:guid}/address", Name = nameof(PostUserAddress))]
    [Authorize]
    public async Task<IActionResult> PostUserAddress([FromRoute] Guid userId, AddressRequest address)
    {
        AddUserAddressRequest request = new() { address = address, userId = userId, nameIdentifier = UserNameIdentifier! };
        var createUserAddressResult = await Sender.Send(request.ToCommand());
        return createUserAddressResult.HasErrors ?
            Problem(createUserAddressResult.Errors) :
            CreatedAtRoute(nameof(GetUserById),
                           new { id = createUserAddressResult?.Data },
                           createUserAddressResult?.Data);
    }

    [HttpPut("{userId:guid}/address", Name = nameof(PutUserAddress))]
    [Authorize]
    public async Task<IActionResult> PutUserAddress([FromRoute] Guid userId, [FromQuery] AddressRequest address, [FromBody] AddressRequest newAddress)
    {
        UpdateUserAddressRequest request = new() { address = address, newAddress = newAddress, userId = userId, nameIdentifier = UserNameIdentifier! };
        var updateUserResult = await Sender.Send(request.ToCommand());
        return updateUserResult.HasErrors ?
            Problem(updateUserResult.Errors) :
        NoContent();
    }

    [HttpDelete("{userId:guid}/address", Name = nameof(DeleteUserAddress))]
    [Authorize]
    public async Task<IActionResult> DeleteUserAddress([FromRoute] Guid userId, [FromQuery] AddressRequest address)
    {
        DeleteUserAddressRequest request = new() { address = address, userId = userId, nameIdentifier = UserNameIdentifier! };
        var deleteUserResult = await Sender.Send(request.ToCommand());
        return deleteUserResult.HasErrors ?
            Problem(deleteUserResult.Errors) :
            NoContent();
    }
}
