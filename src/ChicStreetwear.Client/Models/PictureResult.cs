using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Models.Common;
using ChicStreetwear.Shared.Models.Product;

namespace ChicStreetwear.Client.Models;

internal sealed class PictureResult : Result<PictureModel>
{
    public PictureResult(PictureModel? data, List<Error>? errors, bool hasErrors) : base(data, errors, hasErrors)
    {
    }
}
