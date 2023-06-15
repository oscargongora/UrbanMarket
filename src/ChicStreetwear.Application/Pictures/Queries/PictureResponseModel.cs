using ChicStreetwear.Shared.Interfaces;
using ChicStreetwear.Shared.Models.Common;

namespace ChicStreetwear.Application.Pictures.Queries;
public class PictureResponseModel : PictureModel, IEntityModelBase
{
    public Guid Id { get; set; }
}
