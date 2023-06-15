using ChicStreetwear.Shared.Models.Category;

namespace ChicStreetwear.Api;

public static partial class Requests
{
    public sealed class CreateCategoryRequest : CategoryModel { }
    public sealed class UpdateCategoryRequest : CategoryModel { }
    public sealed class GetCategoriesRequest : GetRequestBase { }
}
