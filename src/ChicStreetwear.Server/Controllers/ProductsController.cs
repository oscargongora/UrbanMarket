using ChicStreetwear.Application.Categories.Commands;
using ChicStreetwear.Application.Products.Commands;
using ChicStreetwear.Application.Products.Queries;
using ChicStreetwear.Server.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ChicStreetwear.Api.Requests;

namespace ChicStreetwear.Server.Controllers;

public class ProductsController : ApiControllerBase
{
    [Authorize]
    [HttpGet]
    [Route("datagrid")]
    public async Task<IActionResult> GetProductsDataGridQuery()
    {
        var query = CreateGetQuery<GetProductsDataGridQuery>();
        query.SellerId = (Guid)UserNameIdentifierAsGuid!;
        var getProductsQueryResult = await Sender.Send(query);
        return getProductsQueryResult.HasErrors ?
            Problem(getProductsQueryResult.Errors) :
            Ok(getProductsQueryResult.Data);
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] GetProductsRequest request)
    {
        var query = new GetProductsQuery { Search = request.Search, Page = request.Page, Take = request.Take };
        var getCategoriesQueryResult = await Sender.Send(query);
        return getCategoriesQueryResult.HasErrors ?
            Problem(getCategoriesQueryResult.Errors) :
            Ok(getCategoriesQueryResult.Data);
    }

    [HttpGet("{id:guid}", Name = nameof(GetProductById))]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var getProductByIdResult = await Sender.Send(new GetProductByIdQuery() { Id = id });
        return getProductByIdResult.HasErrors ?
            Problem(getProductByIdResult.Errors) :
            Ok(getProductByIdResult.Data);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PostProduct(CreateProductRequest request)
    {
        request.SellerId = (Guid)UserNameIdentifierAsGuid!;
        var createProductResult = await Sender.Send(request.ToCommand());
        return createProductResult.HasErrors ?
            Problem(createProductResult.Errors) :
            CreatedAtRoute(nameof(GetProductById),
                           new { id = createProductResult?.Data },
                           createProductResult?.Data);
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, UpdateProductRequest request)
    {
        request.Id = id;
        request.SellerId = (Guid)UserNameIdentifierAsGuid!;
        var updateProductResult = await Sender.Send(request.ToCommand());
        return updateProductResult.HasErrors ?
            Problem(updateProductResult.Errors) :
            NoContent();
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleteProductResult = await Sender.Send(new DeleteProductCommand(id, (Guid)UserNameIdentifierAsGuid!));
        return deleteProductResult.HasErrors ?
            Problem(deleteProductResult.Errors) :
            NoContent();
    }

    [Authorize]
    [HttpDelete("{id:guid}/pictures/{fileName}")]
    public async Task<IActionResult> DeletePicture(Guid id, string fileName)
    {
        var deleteProductPictureResult = await Sender.Send(new DeleteProductPictureCommand() { ProductId = id, FileName = fileName, SellerId = (Guid)UserNameIdentifierAsGuid! });

        return deleteProductPictureResult.HasErrors ?
            Problem(deleteProductPictureResult.Errors) :
            NoContent();
    }
}
