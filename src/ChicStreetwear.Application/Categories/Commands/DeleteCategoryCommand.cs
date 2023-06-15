using ChicStreetwear.Application.Common.Commands;
using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Aggregates.Category;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Shared;
using MediatR;

namespace ChicStreetwear.Application.Categories.Commands;
public sealed class DeleteCategoryCommand : DeleteCommandBase, IRequest<Result<Guid>> { }

public sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result<Guid>>
{
    private readonly IRepository<Category> _categoryRepository;

    public DeleteCategoryCommandHandler(IRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<Guid>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id);
        if (category is null)
        {
            return Result<Guid>.Failed(CategoryErrors.NotFound);
        }

        await _categoryRepository.DeleteAsync(category);
        return Result<Guid>.Succeeded(request.Id);
    }
}
