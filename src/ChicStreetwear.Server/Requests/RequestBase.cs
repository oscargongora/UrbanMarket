namespace ChicStreetwear.Api;

public static partial class Requests
{
    public abstract class GetRequestBase
    {
        public string? Search { get; init; } = default;
        public int? Page { get; init; } = null;
        public int? Take { get; init; } = null;

        public List<KeyValuePair<string, bool>> Sort = new();
    }
}
