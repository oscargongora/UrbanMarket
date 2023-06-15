using ChicStreetwear.Shared;

namespace ChicStreetwear.Domain.Errors;
public static class StockErrors
{
    public static Error InvalidQuantity(string key) => Error.New(key, "The stock quantity must be greater than or equal to zero.", Shared.Enums.ErrorType.BadRequest);

    public static Error InvalidIncreaseQuantity(string key) => Error.New(key, "The quantity to increase must be grater than zero.", Shared.Enums.ErrorType.BadRequest);

    public static Error InvalidDecreaseQuantity(string key) => Error.New(key, "The quantity to decrease must be grater than 1 and less than or equal to the current value of the stock.", Shared.Enums.ErrorType.BadRequest);
}
