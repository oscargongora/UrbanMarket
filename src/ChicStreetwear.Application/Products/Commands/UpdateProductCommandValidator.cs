using ChicStreetwear.Shared.Validators;
using FluentValidation;

namespace ChicStreetwear.Application.Products.Commands;
public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        Include(new ProductModelValidator());
        RuleFor(p => p.Id).Must(id => !id.Equals(Guid.Empty));
    }
}
