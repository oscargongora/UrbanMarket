using ChicStreetwear.Shared.Validators;
using FluentValidation;

namespace ChicStreetwear.Application.Categories.Commands;
public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        Include(new CategoryModelValidator());
    }
}
