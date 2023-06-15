using MudBlazor;
using System.Web;

namespace ChicStreetwear.Client.Helpers;

public static class RoutingHelper
{
    public static string Dashboard => "/manage";
    public static RoutingModel Category = new RoutingModel("categories", "category");
    public static RoutingModel Product = new RoutingModel("products", "product");
    public static RoutingModel Order = new RoutingModel("orders", "order");
}

public class RoutingModel
{
    private readonly string _controllerName;
    private readonly string _entityName;
    public string EntityName => _entityName;
    public string ApiEndpoint => $"api/{_controllerName}";
    public string GetDataGridApiEndpoint<T>(string? Search, ICollection<SortDefinition<T>> sorts, ICollection<FilterDefinition<T>> filters, int? Page, int? Take)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        foreach (var s in sorts)
        {
            query[$"sf{s.Index}"] = s.SortBy;
            query[$"so{s.Index}"] = s.Descending ? "desc" : "asc";
        }
        int filterIndex = 0;
        foreach (var f in filters)
        {
            query[$"fv{filterIndex}"] = f.Value.ToString();
            query[$"ff{filterIndex}"] = f.Title;
            query[$"fo{filterIndex}"] = f.Operator;
            filterIndex++;
        }
        query["search"] = Search;
        query["page"] = Page.ToString();
        query["take"] = Take.ToString();

        return $"{ApiEndpoint}/datagrid?{query}";
    }
    public string GetApiEndpoint(string? Search = null) => $"{ApiEndpoint}?Search={Search ?? string.Empty}";
    public string GetByIdApiEndpoint(Guid Id) => $"{ApiEndpoint}/{Id}";
    public string PutApiEndpoint(Guid Id) => $"{ApiEndpoint}/{Id}";
    public string ListPage => $"/manage/{_controllerName}";
    public string CreatePage => $"/manage/{_controllerName}/create";
    public string EditPage(Guid Id) => $"/manage/{_controllerName}/edit/{Id}";
    public RoutingModel(string controllerName, string entityName)
    {
        _controllerName = controllerName;
        _entityName = entityName;
    }
}




