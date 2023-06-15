using ChicStreetwear.Application.ProductReviews.Commands;
using ChicStreetwear.Application.ProductReviews.Queries;
using ChicStreetwear.Server.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ChicStreetwear.Server.Requests.Requests.ProductReview;

namespace ChicStreetwear.Server.Controllers;

public class ProductReviewsController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProductReviews([FromQuery] GetProductReviewsRequest request)
    {
        var query = request.ToQuery();

        var getProductReviewsQueryResult = await Sender.Send(query);
        return getProductReviewsQueryResult.HasErrors ?
            Problem(getProductReviewsQueryResult.Errors) :
            Ok(getProductReviewsQueryResult.Data);
    }

    [HttpGet("{id:guid}", Name = nameof(GetProductReviewById))]
    public async Task<IActionResult> GetProductReviewById(Guid id)
    {
        var getProductReviewByIdResult = await Sender.Send(new GetProductReviewByIdQuery { Id = id });
        return getProductReviewByIdResult.HasErrors ?
            Problem(getProductReviewByIdResult.Errors) :
            Ok(getProductReviewByIdResult.Data);
    }

    [Authorize]
    [HttpPost(Name = nameof(PostProductReview))]
    public async Task<IActionResult> PostProductReview(CreateProductReviewRequest request)
    {
        request.customerId = Guid.Parse(UserNameIdentifier!);

        var createProductReviewResult = await Sender.Send(request.ToCommand());
        return createProductReviewResult.HasErrors ?
            Problem(createProductReviewResult.Errors) :
            CreatedAtRoute(nameof(GetProductReviewById),
                           new { id = createProductReviewResult?.Data },
                           createProductReviewResult?.Data);
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, UpdateProductReviewRequest request)
    {
        request.id = id;

        var updateProductReviewResult = await Sender.Send(request.ToCommand());
        return updateProductReviewResult.HasErrors ?
            Problem(updateProductReviewResult.Errors) :
        NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleteProductReviewResult = await Sender.Send(new DeleteProductReviewCommand(id));
        return deleteProductReviewResult.HasErrors ?
            Problem(deleteProductReviewResult.Errors) :
            NoContent();
    }
}
