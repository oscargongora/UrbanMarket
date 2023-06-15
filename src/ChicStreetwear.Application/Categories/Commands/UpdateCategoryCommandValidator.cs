using ChicStreetwear.Shared.Validators;
using FluentValidation;

namespace ChicStreetwear.Application.Categories.Commands;
public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
        Include(new CategoryModelValidator());
    }
}
