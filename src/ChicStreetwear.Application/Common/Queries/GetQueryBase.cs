namespace ChicStreetwear.Application.Common.Queries;

public sealed record QuerySortProperty(string Name, string Order);

public sealed record QueryFilterProperty(string Name, string Value, string Operator);

public abstract class GetQueryBase
{
    public string? Search { get; set; } = default;
    public int? Page { get; set; } = null;
    public int? Take { get; set; } = null;
    public SortedDictionary<int, QuerySortProperty> Sorts { get; set; } = new();
    public List<QueryFilterProperty> Filters { get; set; } = new();
}