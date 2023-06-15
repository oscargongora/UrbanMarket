using ChicStreetwear.Shared.Models.Order;
using FluentValidation;

namespace ChicStreetwear.Shared.Validators;
public sealed class OrderModelValidator : AbstractValidator<OrderModel>
{
    public OrderModelValidator()
    {
        RuleFor(o => o.Products)
            .NotEmpty();

        RuleForEach(o => o.Products)
            .ChildRules(op =>
            {
                op.RuleFor(_ => _.ProductId)
                    .NotEmpty();

                op.When(_ => _.VariationId is not null, () =>
                {
                    op.RuleFor(_ => _.Attributes)
                      .NotEmpty();

                    op.RuleForEach(_ => _.Attributes)
                      .ChildRules(a =>
                      {
                          a.RuleFor(_ => _.Name)
                              .NotEmpty();

                          a.RuleFor(_ => _.Value)
                              .NotEmpty();
                      });
                });

                op.RuleFor(_ => _.Name)
                    .NotEmpty();

                op.RuleFor(_ => _.Description)
                    .NotEmpty();

                op.RuleFor(_ => _.Quantity)
                    .NotEmpty()
                    .GreaterThan(0);

                op.RuleFor(_ => _.Price)
                    .NotEmpty()
                    .GreaterThan(0);

                op.RuleFor(_ => _.SellerId)
                    .NotEmpty();
            });

        RuleFor(op => op.DeliveredAddress)
            .SetValidator(new AddressModelValidator());

        RuleFor(op => op.PaymentIntent)
            .NotEmpty();

        RuleFor(op => op.PaymentIntentClientSecret)
            .NotEmpty();

        RuleFor(op => op.ReceiptEmail)
            .NotEmpty();
    }
}

public sealed class AddressModelValidator : AbstractValidator<AddressModel>
{
    public AddressModelValidator()
    {
        When(a => string.IsNullOrWhiteSpace(a.FullName), () =>
        {
            RuleFor(a => a.FirstName)
                .NotEmpty();
            RuleFor(a => a.LastName)
                .NotEmpty();
        });

        RuleFor(a => a.Email)
            .NotEmpty();

        RuleFor(a => a.AddressLine1)
            .NotEmpty();

        RuleFor(a => a.Country)
            .NotEmpty();

        RuleFor(a => a.City)
            .NotEmpty();

        RuleFor(a => a.State)
            .NotEmpty();

        RuleFor(a => a.PostalCode)
            .NotEmpty();
    }
}
