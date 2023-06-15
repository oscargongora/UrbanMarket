using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Enums;

namespace ChicStreetwear.Domain.Errors;
public static class MoneyErrors
{
    public static Error CurrenciesNotMatch => Error.New("Money.CurrenciesNotMatch", "The currencies do not match.", ErrorType.BadRequest);

    public static Error InvalidAmount(string key) => Error.BadRequest(key, "The money amount must be greater than or equals to zero.");
}
