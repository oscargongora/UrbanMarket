using ChicStreetwear.Application.Categories.Queries;
using ChicStreetwear.Application.Common.Queries;
using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace ChicStreetwear.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _sender;
    protected ISender Sender => _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.Count is 0) return Problem();
        if (errors.Any(e => e.Type == ErrorType.Validation))
        {
            ModelStateDictionary modelState = new();
            foreach (var error in errors) modelState.AddModelError(error.Key, error.Message);
            return ValidationProblem(modelState);
        }
        var firstError = errors.First();
        var statusCode = firstError.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.BadRequest => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Internal => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
        return Problem(detail: firstError.Message, statusCode: statusCode, title: firstError.Key);
    }

    protected string? UserNameIdentifier => User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    protected Guid? UserNameIdentifierAsGuid => UserNameIdentifier is null ? null : Guid.Parse(UserNameIdentifier);

    protected TQuery CreateGetQuery<TQuery>() where TQuery : GetQueryBase, new()
    {
        var query = new TQuery();

        SortedDictionary<int, QuerySortProperty> sorts = new();
        List<QueryFilterProperty> filters = new();

        foreach (var key in Request.Query.Keys)
        {
            if (key.StartsWith("sf"))
            {
                int intKey = int.Parse(key.Substring(2));
                string? sf = Request.Query[key];
                string? so = Request.Query[$"so{intKey}"];
                if (sf is not null && so is not null)
                {
                    sorts.Add(intKey, new(sf, so));
                }
            }
            if (key.StartsWith("ff"))
            {
                int intKey = int.Parse(key.Substring(2));
                string? ff = Request.Query[key];
                string? fv = Request.Query[$"fv{intKey}"];
                string? fo = Request.Query[$"fo{intKey}"];
                if (ff is not null && fv is not null && fo is not null)
                {
                    filters.Add(new(ff, fv, fo));
                }
            }
        }

        query.Sorts = sorts;
        query.Filters = filters;
        string? search = Request.Query["search"];
        if (!string.IsNullOrEmpty(search))
        {
            query.Search = Request.Query["search"];
        }

        string? page = Request.Query["page"];
        if (!string.IsNullOrEmpty(page))
        {
            query.Page = int.Parse(page);
        }

        string? take = Request.Query["take"];
        if (!string.IsNullOrEmpty(take))
        {
            query.Take = int.Parse(take);
        }

        return query;
    }
}
