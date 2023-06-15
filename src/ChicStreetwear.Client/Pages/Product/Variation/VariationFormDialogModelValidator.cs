using ChicStreetwear.Client.Pages.Product;
using ChicStreetwear.Shared.Validators;
using FluentValidation;

public sealed class VariationFormDialogModelValidator : AbstractValidator<VariationFormDialogModel>
{
    public VariationFormDialogModelValidator()
    {
        Include(new VariationModelValidator());
    }
}
