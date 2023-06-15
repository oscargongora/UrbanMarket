using ChicStreetwear.Application.Common.Queries;
using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Models.Common;
using MediatR;

namespace ChicStreetwear.Application.Pictures.Queries;
public sealed class GetPictureByIdQuery : GetByIdQueryBase, IRequest<Result<PictureModel>>
{
    public Guid? UserId { get; set; }
}

public sealed class GetPictureByIdQueryHandler : IRequestHandler<GetPictureByIdQuery, Result<PictureModel>>
{

    public GetPictureByIdQueryHandler()
    {
    }

    async Task<Result<PictureModel>> IRequestHandler<GetPictureByIdQuery, Result<PictureModel>>.Handle(GetPictureByIdQuery request, CancellationToken cancellationToken)
    {
        //var picture = await _pictureRepository.FirstOrDefaultAsync(p => p.Id.Equals(request.Id) && p.UserId.Equals(request.UserId), true, cancellationToken);

        //if (picture is null)
        //{
        //    return PictureErrors.NotFound;
        //}

        return Result<PictureModel>.Succeeded(new PictureModel() { FileName=string.Empty, Name=string.Empty, Url=string.Empty, ThumbnailUrl=string.Empty});
    }
}
