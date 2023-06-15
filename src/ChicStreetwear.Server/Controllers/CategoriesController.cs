using ChicStreetwear.Application.Categories.Commands;
using ChicStreetwear.Application.Categories.Queries;
using ChicStreetwear.Server.Mappers;
using ChicStreetwear.Shared.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ChicStreetwear.Api.Requests;

namespace ChicStreetwear.Server.Controllers;

public class CategoriesController : ApiControllerBase
{
    [HttpGet]
    [Route("datagrid")]
    public async Task<IActionResult> GetCategoriesDataGridQuery()
    {
        var query = CreateGetQuery<GetCategoriesDataGridQuery>();

        var getCategoriesQueryResult = await Sender.Send(query);
        return getCategoriesQueryResult.HasErrors ?
            Problem(getCategoriesQueryResult.Errors) :
            Ok(getCategoriesQueryResult.Data);
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories([FromQuery] string? Search)
    {
        var getCategoriesQueryResult = await Sender.Send(new GetCategoriesQuery() { Search = Search });
        return getCategoriesQueryResult.HasErrors ?
            Problem(getCategoriesQueryResult.Errors) :
            Ok(getCategoriesQueryResult.Data);
    }

    [HttpGet("{id:guid}", Name = nameof(GetCategoryById))]
    public async Task<IActionResult> GetCategoryById(Guid id)
    {
        var getCategoryByIdResult = await Sender.Send(new GetCategoryByIdQuery { Id = id });
        return getCategoryByIdResult.HasErrors ?
            Problem(getCategoryByIdResult.Errors) :
            Ok(getCategoryByIdResult.Data);
    }

    [Authorize(Roles = IdentityDefaults.ADMINISTRATOR_ROLE)]
    [HttpPost]
    public async Task<IActionResult> PostCategory(CreateCategoryRequest request)
    {
        var createCategoryResult = await Sender.Send(request.ToCommand());
        return createCategoryResult.HasErrors ?
            Problem(createCategoryResult.Errors) :
            CreatedAtRoute(nameof(GetCategoryById),
                           new { id = createCategoryResult?.Data },
                           createCategoryResult?.Data);
    }

    [Authorize(Roles = IdentityDefaults.ADMINISTRATOR_ROLE)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> PutCategory(Guid id, UpdateCategoryRequest request)
    {
        request.Id = id;
        var updateCategoryResult = await Sender.Send(request.ToCommand());
        return updateCategoryResult.HasErrors ?
            Problem(updateCategoryResult.Errors) :
            NoContent();
    }

    [Authorize(Roles = IdentityDefaults.ADMINISTRATOR_ROLE)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleteCategoryResult = await Sender.Send(new DeleteCategoryCommand() { Id = id });
        return deleteCategoryResult.HasErrors ?
            Problem(deleteCategoryResult.Errors) :
            NoContent();
    }
}
