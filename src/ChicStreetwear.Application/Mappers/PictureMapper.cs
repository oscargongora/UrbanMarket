using ChicStreetwear.Application.Pictures.Queries;
using ChicStreetwear.Domain.ValueObjects;
using Riok.Mapperly.Abstractions;

namespace ChicStreetwear.Application.Mappers;

[Mapper]
internal static partial class PictureMapper
{
    internal static partial PictureResponseModel ToPictureResponseModel(this Picture picture);

}
