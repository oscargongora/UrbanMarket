using ChicStreetwear.Shared.Models.Product;
using FluentValidation;

namespace ChicStreetwear.Shared.Validators;
public sealed class VariationModelValidator : AbstractValidator<VariationModel>
{
    public VariationModelValidator()
    {
        RuleFor(v => v.Stock).GreaterThanOrEqualTo(0);
        RuleFor(v => v.PurchasedPrice).GreaterThanOrEqualTo(0);
        RuleFor(v => v.RegularPrice).GreaterThanOrEqualTo(0);
        When(v => v.SalePrice is not null, () =>
        {
            RuleFor(v => v.SalePrice).GreaterThanOrEqualTo(0);

            RuleFor(v => v.SalePrice)
            .Must((v, salePrice) => salePrice < v.RegularPrice)
            .WithMessage("Sale price must be less than regular price");

        });

        RuleForEach(v => v.Attributes)
           .ChildRules(av =>
           {
               av.RuleFor(_ => _.Value)
               .NotNull().WithMessage(_ => $"The {_.Attribute.Name} attribute is required")
               .NotEmpty().WithMessage(_ => $"The {_.Attribute.Name} attribute is required");

           });
    }
}
