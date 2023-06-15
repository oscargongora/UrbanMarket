using ChicStreetwear.Application.Carts.Commands;

namespace ChicStreetwear.Api;

public static partial class Requests
{
    public abstract class CartProductCommandRequestBase
    {
        public string? nameIdentifier { get; set; }
        public Guid? cartId { get; set; }
        public required Guid productId { get; set; }
        public Guid? variationId { get; set; }
        public required int quantity { get; set; }
    }
    public sealed class AddCartProductRequest : CartProductCommandRequestBase
    {
        public AddCartProductCommand ToCommand() => new(productId, variationId, quantity, cartId, nameIdentifier);
    }

    public sealed class UpdateCartProductRequest : CartProductCommandRequestBase
    {
        public required Guid cartProductId { get; set; }
        public UpdateCartProductCommand ToCommand() => new(cartProductId, productId, variationId, quantity, (Guid)cartId!, nameIdentifier);
    }

    public sealed class DeleteCartProductRequest
    {
        public required Guid cartProductId { get; set; }
        public DeleteCartProductCommand ToCommand(Guid cartId) => new(cartId, cartProductId);
    }

    public sealed class GetCartsRequest : GetRequestBase { }
}
