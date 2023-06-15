using ChicStreetwear.Domain.ValueObjects;

namespace ChicStreetwear.Application.Common.Responses;
public record PictureResponse
{
    public required string Name { get; set; }
    public required string Url { get; set; }
    public static PictureResponse? FromPicture(Picture? coverPicture)
    {
        return coverPicture is null ? null : new() { Name = coverPicture.Name, Url = coverPicture.Url };
    }
}
