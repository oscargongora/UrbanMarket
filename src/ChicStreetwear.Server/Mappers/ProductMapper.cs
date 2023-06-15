using ChicStreetwear.Application.Products.Commands;
using Riok.Mapperly.Abstractions;
using static ChicStreetwear.Api.Requests;

namespace ChicStreetwear.Server.Mappers;

[Mapper]
internal static partial class ProductMapper
{
    internal static partial CreateProductCommand ToCommand(this CreateProductRequest request);
    internal static partial UpdateProductCommand ToCommand(this UpdateProductRequest request);
}
