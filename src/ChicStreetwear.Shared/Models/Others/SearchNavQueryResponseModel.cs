namespace ChicStreetwear.Shared.Models.Others;
public sealed class SearchNavQueryResponseModel
{
    public required Guid Id { get; set; }
    public required string Type { get; set; }
    public required string Title { get; set; }
    public string? ThumbnailUrl { get; set; }
}
