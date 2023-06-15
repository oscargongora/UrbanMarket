using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Application.Common.Queries;
using ChicStreetwear.Application.Mappers;
using ChicStreetwear.Domain.Aggregates.Category;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Shared;
using MediatR;

namespace ChicStreetwear.Application.Categories.Queries;
public sealed class GetCategoryByIdQuery : GetByIdQueryBase, IRequest<Result<CategoryResponseModel>> { }

public sealed class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Result<CategoryResponseModel>>
{
    private readonly IRepository<Category> _categoryRepository;

    public GetCategoryByIdQueryHandler(IRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<CategoryResponseModel>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id);
        if (category is null)
        {
            return Result<CategoryResponseModel>.Failed(CategoryErrors.NotFound);
        }
        return Result<CategoryResponseModel>.Succeeded(category.ToCategoryResponseModel());
    }
}
