using ChicStreetwear.Shared.Validators;
using FluentValidation;

namespace ChicStreetwear.Application.Orders.Commands;
public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        Include(new OrderModelValidator());
    }
}
