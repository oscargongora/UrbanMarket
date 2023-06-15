using ChicStreetwear.Application.Products.Commands;
using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Validators;
using FluentValidation;

namespace ChicStreetwear.Application.Products.Commands;
public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        Include(new ProductModelValidator());
        RuleFor(p => p.Id).Must(id => id.Equals(Guid.Empty));
    }
}
