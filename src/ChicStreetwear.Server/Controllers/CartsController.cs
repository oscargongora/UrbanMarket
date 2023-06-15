using ChicStreetwear.Application.Carts.Commands;
using ChicStreetwear.Application.Carts.Queries;
using ChicStreetwear.Application.Categories.Queries;
using ChicStreetwear.Domain.Aggregates.Cart.Entities;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static ChicStreetwear.Api.Requests;

namespace ChicStreetwear.Server.Controllers;

public class CartsController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCarts([FromQuery] GetCartsRequest request)
    {
        var query = new GetCartsQuery { Search = request.Search, Page = request.Page, Take = request.Take };

        var getCartsQueryResult = await Sender.Send(query);
        return getCartsQueryResult.HasErrors ?
            Problem(getCartsQueryResult.Errors) :
            Ok(getCartsQueryResult.Data);
    }

    [HttpGet("{id:guid}", Name = nameof(GetCartById))]
    public async Task<IActionResult> GetCartById(Guid id)
    {
        var getCartByIdResult = await Sender.Send(new GetCartByIdQuery(id));
        return getCartByIdResult.HasErrors ?
            Problem(getCartByIdResult.Errors) :
            Ok(getCartByIdResult.Data);
    }

    [HttpPost(Name = nameof(PostCart))]
    public async Task<IActionResult> PostCart(AddCartProductRequest request)
    {
        request.nameIdentifier = UserNameIdentifier;
        var command = request.ToCommand();

        var createCartProductResult = await Sender.Send(command);
        return createCartProductResult.HasErrors ?
            Problem(createCartProductResult.Errors) :
            CreatedAtRoute(nameof(GetCartById),
                           new { id = createCartProductResult?.Data?.cartId },
                           createCartProductResult?.Data);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, UpdateCartProductRequest request)
    {
        request.cartId = id;
        request.nameIdentifier = UserNameIdentifier;

        var updateCartProductResult = await Sender.Send(request.ToCommand());
        return updateCartProductResult.HasErrors ?
            Problem(updateCartProductResult.Errors) :
        NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, DeleteCartProductRequest request)
    {
        var deleteCartResult = await Sender.Send(request.ToCommand(id));
        return deleteCartResult.HasErrors ?
            Problem(deleteCartResult.Errors) :
            NoContent();
    }
}
