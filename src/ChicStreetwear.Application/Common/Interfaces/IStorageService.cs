using Microsoft.AspNetCore.Http;

namespace ChicStreetwear.Application.Common.Interfaces;
public interface IStorageService
{
    Task<(Uri pictureUri, Uri thumbnailUri)?> UploadPicture(IFormFile file, string fileName);
    Task DeletePicture(string fileName);
}