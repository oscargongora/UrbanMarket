using ChicStreetwear.Shared.Interfaces;

namespace ChicStreetwear.Shared.Models.Components;
public sealed class PaginatedListModel<TModel>
{
    public List<TModel> Items { get; set; }
    public int TotalItems { get; set; }

    public PaginatedListModel(List<TModel> items, int totalItems)
    {
        Items = items;
        TotalItems = totalItems;
    }
}
