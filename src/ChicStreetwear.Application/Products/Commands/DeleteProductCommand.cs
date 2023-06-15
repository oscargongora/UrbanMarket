using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Aggregates.Product;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Domain.Events.Product;
using ChicStreetwear.Shared;
using MediatR;

namespace ChicStreetwear.Application.Categories.Commands;
public record DeleteProductCommand(Guid Id, Guid SellerId) : IRequest<Result<Guid>>;

public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result<Guid>>
{
    private readonly IRepository<Product> _productRepository;

    public DeleteProductCommandHandler(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<Guid>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);
        if (product is null)
        {
            return ProductErrors.ProductNotFound;
        }

        if(!product.SellerId.Equals(request.SellerId))
        {
            return Error.New("DeleteProduct", "The product could not be deleted because you are not the owner.", Shared.Enums.ErrorType.Unauthorized);
        }

        await _productRepository.DeleteAsync(product);
        return Result<Guid>.Succeeded(request.Id);
    }
}
