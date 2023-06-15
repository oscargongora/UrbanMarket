using ChicStreetwear.Shared.Models.Category;
using FluentValidation;

namespace ChicStreetwear.Shared.Validators;
public sealed class CategoryModelValidator : AbstractValidator<CategoryModel>
{
    public CategoryModelValidator()
    {
        RuleFor(category => category.Name)
            .MaximumLength(Constants.Fields.NAME_MAXLENGTH)
            .NotNull()
            .NotEmpty();
        RuleFor(category => category.Description)
            .MaximumLength(Constants.Fields.DESCRIPTION_MAXLENGTH)
            .NotNull()
            .NotEmpty();
    }
}
