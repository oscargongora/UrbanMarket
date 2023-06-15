using ChicStreetwear.Shared.Models.Product;

namespace ChicStreetwear.Api;

public static partial class Requests
{
    public sealed class CreateProductRequest : ProductModel { }
    public sealed class UpdateProductRequest : ProductModel { }
    public sealed class GetProductsRequest : GetRequestBase { }
}

