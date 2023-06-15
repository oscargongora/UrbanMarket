using ChicStreetwear.Shared.Models.Product;
using FluentValidation;

namespace ChicStreetwear.Shared.Validators;
public sealed class AttributeModelValidator : AbstractValidator<AttributeModel>
{
    public AttributeModelValidator()
    {
        RuleFor(a => a.Name).NotNull().NotEmpty();
    }
}
