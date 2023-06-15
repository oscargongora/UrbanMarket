using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Aggregates.Category;
using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Models.Category;
using MediatR;

namespace ChicStreetwear.Application.Categories.Commands;

public sealed class CreateCategoryCommand : CategoryModel, IRequest<Result<Guid>> { }

public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<Guid>>
{
    private readonly IRepository<Category> _categoryRepository;

    public CreateCategoryCommandHandler(IRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryResult = Category.New(request.Name, request.Description);
        if (categoryResult.HasErrors)
        {
            return Result<Guid>.Failed(categoryResult.Errors);
        }

        await _categoryRepository.AddAsync(categoryResult.Data, cancellationToken);

        return Result<Guid>.Succeeded(categoryResult.Data.Id);
    }
}

