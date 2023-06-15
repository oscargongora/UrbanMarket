using ChicStreetwear.Application.Pictures.Queries;
using Riok.Mapperly.Abstractions;
using static ChicStreetwear.Api.Requests;

namespace ChicStreetwear.Server.Mappers;

[Mapper]
internal static partial class PictureMapper
{
    internal static partial GetPicturesQuery ToQuery(this GetPicturesRequest request);
}
