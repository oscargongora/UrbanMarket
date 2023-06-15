namespace ChicStreetwear.Shared.Models.Common;
public class PictureModel
{
    //public  string Name { get; set; }
    //public  string FileName { get; set; }
    //public  string Url { get; set; }
    //public  Guid UserId { get; set; }

    public required string FileName { get; set; }
    public required string Name { get; set; }
    public required string Url { get; set; }
    public required string ThumbnailUrl { get; set; }
    //public required Guid UserId { get; set; }
}
