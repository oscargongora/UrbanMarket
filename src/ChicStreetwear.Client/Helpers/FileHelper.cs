using Microsoft.AspNetCore.Components.Forms;

namespace ChicStreetwear.Client.Helpers;

public static class FileHelper
{
    public static async Task<string> ReadImageFromFile(IBrowserFile file, bool requestImageFileAsync = true, int maxWidth = 300, int maxHeight = 300)
    {
        if (requestImageFileAsync)
        {
            //var resizedImage = await file.RequestImageFileAsync(file.ContentType, maxWidth, maxHeight);
            var buffer = new byte[file.Size];
            await file.OpenReadStream().ReadAsync(buffer);
            return $"data:{file.ContentType};base64,{Convert.ToBase64String(buffer)}";
        }
        else
        {
            var buffer = new byte[file.Size];
            await file.OpenReadStream().ReadAsync(buffer);
            return $"data:{file.ContentType};base64,{Convert.ToBase64String(buffer)}";
        }
    }
}
