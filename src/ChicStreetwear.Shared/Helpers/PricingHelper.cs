namespace ChicStreetwear.Shared.Helpers;

public static class PricingHelper
{
    public static decimal? Profit(decimal? SalePrice, decimal? RegularPrice, decimal? PurchasedPrice) =>
        (SalePrice ?? RegularPrice ?? decimal.Zero) - (PurchasedPrice ?? decimal.Zero);

    public static decimal? Margin(decimal? SalePrice, decimal? RegularPrice, decimal? PurchasedPrice)
    {
        var profit = Profit(SalePrice, RegularPrice, PurchasedPrice);

        if (SalePrice is not null)
        {
            return profit / (SalePrice == 0 ? 1 : SalePrice);
        }

        if (RegularPrice is not null)
        {
            return profit / (RegularPrice == 0 ? 1 : RegularPrice);
        }
        return 0;
    }
}
