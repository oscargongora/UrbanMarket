using ChicStreetwear.Shared.Models.Product;
using FluentValidation;

namespace ChicStreetwear.Shared.Validators;
public sealed class ProductModelValidator : AbstractValidator<ProductModel>
{
    public ProductModelValidator()
    {
        RuleFor(p => p.Name).NotNull().NotEmpty().MaximumLength(Constants.Fields.NAME_MAXLENGTH);
        RuleFor(p => p.Description).NotNull().NotEmpty().MaximumLength(Constants.Fields.DESCRIPTION_MAXLENGTH);

        When(p => !p.HasAttributes, () =>
        {
            RuleFor(p => p.PurchasedPrice).GreaterThanOrEqualTo(0);
            RuleFor(p => p.RegularPrice).GreaterThanOrEqualTo(0);

            When(p => p.SalePrice is not null, () =>
            {
                RuleFor(p => p.SalePrice).GreaterThanOrEqualTo(0);

                RuleFor(p => p.SalePrice).Must((p, salePrice) => salePrice < p.RegularPrice)
                .WithMessage("Sale price must be less than regular price");
            });

            RuleFor(p => p.Stock).NotNull().GreaterThanOrEqualTo(0);
        });

        When(p => p.HasAttributes, () =>
        {
            RuleFor(p => p.Attributes).NotNull().NotEmpty();
            RuleFor(p => p.Variations).NotNull().NotEmpty();
        });
    }
}
