using ChicStreetwear.Application.Categories.Commands;
using ChicStreetwear.Application.Categories.Queries;
using Riok.Mapperly.Abstractions;
using static ChicStreetwear.Api.Requests;

namespace ChicStreetwear.Server.Mappers;

[Mapper]
internal static partial class CategoryMapper
{
    internal static partial CreateCategoryCommand ToCommand(this CreateCategoryRequest request);
    internal static partial UpdateCategoryCommand ToCommand(this UpdateCategoryRequest request);
    internal static partial GetCategoriesDataGridQuery ToQuery(this GetCategoriesRequest request);
}
