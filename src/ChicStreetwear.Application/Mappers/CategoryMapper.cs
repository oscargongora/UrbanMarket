using ChicStreetwear.Application.Categories.Queries;
using ChicStreetwear.Domain.Aggregates.Category;
using ChicStreetwear.Shared.Models.Components;
using Riok.Mapperly.Abstractions;

namespace ChicStreetwear.Application.Mappers;

[Mapper]
internal static partial class CategoryMapper
{
    internal static partial CategoryResponseModel ToCategoryResponseModel(this Category category);
    internal static CategoryDataGridItemModel ToCategoryItemDataGridModel(this Category category, int products) => new() { Id = category.Id, Products = products, Name = category.Name, Description = category.Description };
}
