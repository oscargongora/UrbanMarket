using ChicStreetwear.Application.Pictures.Commands;
using ChicStreetwear.Application.Pictures.Queries;
using ChicStreetwear.Server.Mappers;
using ChicStreetwear.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ChicStreetwear.Api.Requests;

namespace ChicStreetwear.Server.Controllers;

public sealed class PicturesController : ApiControllerBase
{
    [Authorize]
    [HttpPost]
    [Route("upload")]
    public async Task<IActionResult> Upload()
    {
        var files = (await Request.ReadFormAsync()).Files.ToList();

        if (files is null)
        {
            return Problem(new List<Error>() { Error.BadRequest("UploadFile", "Files are required.") });
        }

        var uploadPicturesResult = await Sender.Send(new UploadPicturesCommand() { Files = files });

        return Ok(uploadPicturesResult);
    }
}
