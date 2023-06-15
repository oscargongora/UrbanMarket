using ChicStreetwear.Application.Common.Interfaces;
using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Shared;
using MediatR;

namespace ChicStreetwear.Application.Products.Commands;
public sealed class DeleteProductPictureCommand : IRequest<Result<Guid>>
{
    public required Guid ProductId { get; set; }
    public required string FileName { get; set; }
    public required Guid SellerId { get; set; }
}

public sealed class DeleteProductPictureCommandHandler : IRequestHandler<DeleteProductPictureCommand, Result<Guid>>
{
    private readonly IProductRepository _productRepository;
    private readonly IStorageService _storageService;

    public DeleteProductPictureCommandHandler(IProductRepository productRepository, IStorageService storageService)
    {
        _productRepository = productRepository;
        _storageService = storageService;
    }

    async Task<Result<Guid>> IRequestHandler<DeleteProductPictureCommand, Result<Guid>>.Handle(DeleteProductPictureCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product is null)
        {
            return ProductErrors.ProductNotFound;
        }

        if (!product.SellerId.Equals(request.SellerId))
        {
            return Error.New("DeleteProductPicture", "The picture could not be deleted because you are not the owner of the product.", Shared.Enums.ErrorType.Unauthorized);
        }

        await _storageService.DeletePicture(request.FileName);

        var result = product.RemovePicture(request.FileName);
        if (result.HasErrors)
        {
            return result.Errors;
        }
        await _productRepository.UpdateAsync(product);
        return Result<Guid>.Succeeded(product.Id);
    }
}
