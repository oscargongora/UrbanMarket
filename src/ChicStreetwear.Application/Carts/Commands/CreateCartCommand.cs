using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Aggregates.Cart;
using ChicStreetwear.Shared;
using MediatR;

namespace ChicStreetwear.Application.Carts.Commands;
public sealed record CreateCartResponse(Guid cartId, Guid? customerId);

public sealed record CreateCartCommand(string? customerId) : IRequest<Result<CreateCartResponse>>;

public sealed class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, Result<CreateCartResponse>>
{
    private readonly IRepository<Cart> _cartRepository;

    public CreateCartCommandHandler(IRepository<Cart> cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<Result<CreateCartResponse>> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        Guid? customerId = null;
        if (request.customerId is not null)
        {
            customerId = Guid.Parse(request.customerId);
        }

        var cart = Cart.New(customerId);

        await _cartRepository.AddAsync(cart);

        return Result<CreateCartResponse>.Succeeded(new CreateCartResponse(cart.Id, customerId));
    }
}