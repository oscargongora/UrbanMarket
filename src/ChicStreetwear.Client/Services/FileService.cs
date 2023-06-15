using ChicStreetwear.Client.Models;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;

namespace ChicStreetwear.Client.Services;

internal sealed class FileService
{
    private const string UPLOAD_PICTURES_ADDRESS = "api/pictures/upload";
    private readonly HttpClient _httpClient;

    public FileService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PictureResult>> UploadPictures(List<IBrowserFile> files)
    {
        MultipartFormDataContent multipartFormData = new();
        foreach (var file in files)
        {
            multipartFormData.Add(new StreamContent(file.OpenReadStream()), file.Name, file.Name);
        }
        var response = await _httpClient.PostAsync(UPLOAD_PICTURES_ADDRESS, multipartFormData);
        return await response.Content.ReadFromJsonAsync<List<PictureResult>>() ?? new();
    }
}
