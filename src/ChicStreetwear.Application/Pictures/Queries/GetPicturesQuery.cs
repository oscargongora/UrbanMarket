using ChicStreetwear.Application.Common.Queries;
using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Models.Common;
using ChicStreetwear.Shared.Models.Components;
using MediatR;
using System.Linq.Expressions;

namespace ChicStreetwear.Application.Pictures.Queries;
public sealed class GetPicturesQuery : GetQueryBase, IRequest<Result<PaginatedListModel<PictureModel>>>
{
    public Guid? UserId { get; set; }
}

public sealed class GetPicturesQueryHandler : IRequestHandler<GetPicturesQuery, Result<PaginatedListModel<PictureModel>>>
{

    public GetPicturesQueryHandler()
    {
    }

    async Task<Result<PaginatedListModel<PictureModel>>> IRequestHandler<GetPicturesQuery, Result<PaginatedListModel<PictureModel>>>.Handle(GetPicturesQuery request, CancellationToken cancellationToken)
    {
        //List<Expression<Func<Picture, bool>>> predicates = new();
        //if (request.Search is not null)
        //{
        //    predicates.Add(c => c.Name.Contains(request.Search));
        //}
        //if (request.UserId is not null)
        //{
        //    predicates.Add(c => c.UserId.Equals(request.UserId));
        //}

        //var pictures = await _pictureRepository.PaginatedListAsync(predicates, new(), request.Page ?? 1, request.Take ?? 10, true, cancellationToken);

        return Result<PaginatedListModel<PictureModel>>.Succeeded(new(new(), 0));
    }
}
