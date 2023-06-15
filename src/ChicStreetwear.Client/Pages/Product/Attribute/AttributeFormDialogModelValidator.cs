using ChicStreetwear.Shared.Validators;
using FluentValidation;

namespace ChicStreetwear.Client.Pages.Product;

public class AttributeFormDialogModelValidator : AbstractValidator<AttributeFormDialogModel>
{
    public AttributeFormDialogModelValidator()
{
    Include(new AttributeModelValidator());
}
}
