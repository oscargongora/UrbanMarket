using ChicStreetwear.Domain.Aggregates.Cart;
using ChicStreetwear.Shared;

namespace ChicStreetwear.Domain.Errors;

public static class CartErrors
{
    public static Error ProductNotFound => Error.NotFound("Cart.ProductNotFound", "Product not found in the cart.");

    public static Error CartNotFound => Error.NotFound("Cart.NotFound", "Cart not found.");

    public static Error InvalidProductQuantity(string key) => Error.BadRequest(key, "Invalid quantity for a product in the shopping cart.");

    public static Error RequestedQuantityExceedsQuantityInStock => Error.BadRequest("Cart.RequestedQuantityExceedsQuantityInStock", "The quantity of the product requested exceeds the quantity available in stock.");

    public static Error MissingAttributes => Error.BadRequest("AddCartProduct", "Missing attributes for variation.");
}