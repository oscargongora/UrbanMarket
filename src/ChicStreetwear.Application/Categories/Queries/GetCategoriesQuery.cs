using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Application.Mappers;
using ChicStreetwear.Shared;
using MediatR;

namespace ChicStreetwear.Application.Categories.Queries;
public sealed class GetCategoriesQuery : IRequest<Result<List<CategoryResponseModel>>>
{
    public string? Search { get; set; } = default;
}

public sealed class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, Result<List<CategoryResponseModel>>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<List<CategoryResponseModel>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        request.Search ??= string.Empty;
        var categories = await _categoryRepository.ListAsync(c => c.Name.Contains(request.Search) || c.Description.Contains(request.Search), true, cancellationToken);
        return Result<List<CategoryResponseModel>>.Succeeded(categories.ConvertAll(c => c.ToCategoryResponseModel()));
    }
}
