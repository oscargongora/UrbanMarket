using ChicStreetwear.Application.Orders.Queries;
using ChicStreetwear.Server.Mappers;
using ChicStreetwear.Shared.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ChicStreetwear.Server.Requests.Requests.Order;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ChicStreetwear.Server.Controllers;

public sealed class OrdersController : ApiControllerBase
{
    [Authorize]
    [HttpGet]
    [Route("datagrid")]
    public async Task<IActionResult> GetOrdersDataGridQuery()
    {
        var query = CreateGetQuery<GetOrdersDataGridQuery>();

        query.UserId = (Guid)UserNameIdentifierAsGuid!;
        query.IsAdministrator = User.IsInRole(IdentityDefaults.ADMINISTRATOR_ROLE);

        var getProductsQueryResult = await Sender.Send(query);
        return getProductsQueryResult.HasErrors ?
            Problem(getProductsQueryResult.Errors) :
            Ok(getProductsQueryResult.Data);
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] GetOrdersRequest request)
    {
        var getOrdersQueryResult = await Sender.Send(request.ToQuery());
        return getOrdersQueryResult.HasErrors ?
            Problem(getOrdersQueryResult.Errors) :
            Ok(getOrdersQueryResult.Data);
    }

    [HttpGet("{id:guid}", Name = nameof(GetOrderById))]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        var query = new GetOrderByIdQuery() { Id = id };
        query.UserId = (Guid)UserNameIdentifierAsGuid!;
        query.IsAdministrator = User.IsInRole(IdentityDefaults.ADMINISTRATOR_ROLE);
        var getOrderByIdResult = await Sender.Send(query);
        return getOrderByIdResult.HasErrors ?
            Problem(getOrderByIdResult.Errors) :
            Ok(getOrderByIdResult.Data);
    }

    [HttpPost]
    public async Task<IActionResult> PostOrder(CreateOrderRequest request)
    {
        var createOrderProductResult = await Sender.Send(request.ToCommand());
        return createOrderProductResult.HasErrors ?
            Problem(createOrderProductResult.Errors) :
            CreatedAtRoute(nameof(GetOrderById),
                           new { id = createOrderProductResult?.Data },
                           createOrderProductResult?.Data);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> PutOrder(Guid id, UpdateOrderRequest request)
    {
        request.Id = id;
        var updateOrderProductResult = await Sender.Send(request.ToCommand());
        return updateOrderProductResult.HasErrors ?
            Problem(updateOrderProductResult.Errors) :
        NoContent();
    }
}
