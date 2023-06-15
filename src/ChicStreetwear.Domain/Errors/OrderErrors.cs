using ChicStreetwear.Shared;

namespace ChicStreetwear.Domain.Errors;

public static class OrderErrors
{
    public static Error InvalidProductQuantity(string key) => Error.BadRequest(key, "Invalid quantity for a product in the order.");

    public static Error CartNotFound => Error.NotFound("Order.CartNotFound", "The order could not be created because the shopping cart was not found");

    public static Error OrderNotFound => Error.NotFound("Order.NotFound", "Order not found.");
}