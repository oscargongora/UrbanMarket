using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Aggregates.Category;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Interfaces;
using ChicStreetwear.Shared.Models.Category;
using MediatR;

namespace ChicStreetwear.Application.Categories.Commands;
public sealed class UpdateCategoryCommand : CategoryModel, IRequest<Result<Guid>>
{
}

public sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result<Guid>>
{
    private readonly IRepository<Category> _categoryRepository;

    public UpdateCategoryCommandHandler(IRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<Guid>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category is null)
        {
            return Result<Guid>.Failed(CategoryErrors.NotFound);
        }

        category.Update(request.Name, request.Description);
        await _categoryRepository.UpdateAsync(category, cancellationToken);
        return Result<Guid>.Succeeded(category.Id);
    }
}
