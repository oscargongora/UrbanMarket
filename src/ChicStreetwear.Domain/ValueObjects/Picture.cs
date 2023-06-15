using ChicStreetwear.Domain.Common;

namespace ChicStreetwear.Domain.ValueObjects;
public sealed class Picture : ValueObject
{
    public string FileName { get; private set; }
    public string Name { get; private set; }
    public string Url { get; private set; }
    public string ThumbnailUrl { get; private set; }

    private Picture(string fileName, string name, string url, string thumbnailUrl)
    {
        FileName = fileName;
        Name = name;
        Url = url;
        ThumbnailUrl = thumbnailUrl;
    }

    public static Picture New(string fileName, string name, string url, string thumbnailUrl)
    {
        return new Picture(fileName, name, url, thumbnailUrl);
    }
    public override IEnumerable<object> GetEqualtyComponents()
    {
        yield return FileName;
    }
}

