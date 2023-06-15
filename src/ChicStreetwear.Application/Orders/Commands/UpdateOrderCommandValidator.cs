using ChicStreetwear.Shared.Validators;
using FluentValidation;

namespace ChicStreetwear.Application.Orders.Commands;
public sealed class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(o => o.Id)
            .NotEmpty();
        Include(new OrderModelValidator());
    }
}
