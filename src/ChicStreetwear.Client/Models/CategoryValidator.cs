using ChicStreetwear.Shared.Validators;
using FluentValidation;

namespace ChicStreetwear.Client.Models;

public sealed class CreateCategoryModelValidator : AbstractValidator<CreateCategoryModel>
{
    public CreateCategoryModelValidator()
    {
        Include(new CategoryModelValidator());
    }
}

public sealed class UpdateCategoryModelValidator : AbstractValidator<UpdateCategoryModel>
{
    public UpdateCategoryModelValidator()
    {
        Include(new CategoryModelValidator());
    }
}

