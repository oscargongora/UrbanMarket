using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ChicStreetwear.Application.Common.Interfaces;
using ChicStreetwear.Infrastructure.Options;
using ChicStreetwear.Shared.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.Advanced;

namespace ChicStreetwear.Infrastructure.Files;
public sealed class StorageService : IStorageService
{
    private readonly IOptionsMonitor<CloudStorageOptions> _storageOptions;
    private readonly ILogger<StorageService> _logger;

    public StorageService(IOptionsMonitor<CloudStorageOptions> storageOptions, ILogger<StorageService> logger)
    {
        _storageOptions = storageOptions;
        _logger = logger;
    }


    public async Task<(Uri pictureUri, Uri thumbnailUri)?> UploadPicture(IFormFile file, string fileName)
    {
        try
        {
            if (file.Length > 0)
            {
                var container = new BlobContainerClient(_storageOptions.CurrentValue.ConnectionString, _storageOptions.CurrentValue.ContainerName);

                var pictureBlob = container.GetBlobClient(fileName);

                BlobClient? thumbnailBlob = null;
                if (PicturesOptions.CreateThumbnail)
                {
                    thumbnailBlob = container.GetBlobClient($"thumbnail-{fileName}");
                }

                using (var fileStream = file.OpenReadStream())
                {
                    await pictureBlob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = file.ContentType });

                    if (thumbnailBlob is not null)
                    {
                        fileStream.Seek(0, SeekOrigin.Begin);

                        using (var image = await Image.LoadAsync(fileStream))
                        {
                            var resizeOptions = new ResizeOptions() { Mode = ResizeMode.Min, Size = new Size(PicturesOptions.ThumbnailSize) };
                            image.Mutate(x => x.Resize(resizeOptions));

                            using (var thumbnailStream = new MemoryStream())
                            {
                                await image.SaveAsync(thumbnailStream, image.DetectEncoder(file.Name));
                                thumbnailStream.Seek(0, SeekOrigin.Begin);
                                await thumbnailBlob.UploadAsync(thumbnailStream, new BlobHttpHeaders { ContentType = file.ContentType });
                            }
                        }
                    }
                }

                return (pictureBlob.Uri, thumbnailBlob?.Uri ?? pictureBlob.Uri);
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.Message);
        }
        return default;
    }

    public async Task DeletePicture(string fileName)
    {
        try
        {
            var container = new BlobContainerClient(_storageOptions.CurrentValue.ConnectionString, _storageOptions.CurrentValue.ContainerName);

            var pictureBlob = container.GetBlobClient(fileName);
            await pictureBlob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
            if (PicturesOptions.CreateThumbnail)
            {
                var thumbnailBlob = container.GetBlobClient($"thumbnail-{fileName}");
                await thumbnailBlob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.Message);
        }
    }
}
