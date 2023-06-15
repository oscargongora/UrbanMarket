using ChicStreetwear.Application.Common.Interfaces;
using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Models.Common;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ChicStreetwear.Application.Pictures.Commands;
public sealed class UploadPicturesCommand : IRequest<List<Result<PictureModel>>>
{
    public required List<IFormFile> Files { get; set; }
}

public sealed class UploadPicturesCommandHandler : IRequestHandler<UploadPicturesCommand, List<Result<PictureModel>>>
{
    private readonly IStorageService _storageService;

    public UploadPicturesCommandHandler(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<List<Result<PictureModel>>> Handle(UploadPicturesCommand request, CancellationToken cancellationToken)
    {
        List<Result<PictureModel>> results = new();
        foreach (var file in request.Files)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var uris = await _storageService.UploadPicture(file, fileName);
            if (uris is null)
            {
                results.Add(Error.New("UploadPicture", "An error occurred while uploading the picture", Shared.Enums.ErrorType.Internal));
                break;
            }
            PictureModel picture = new() { FileName = fileName, Name = file.FileName, Url = uris.Value.pictureUri.ToString(), ThumbnailUrl = uris.Value.thumbnailUri.ToString() };
            results.Add(Result<PictureModel>.Succeeded(picture));
        }

        return results;
    }
}
