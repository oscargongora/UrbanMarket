using ChicStreetwear.Domain.Common;

namespace ChicStreetwear.Domain.Aggregates.Product.ValueObjects;
public sealed class Stock : ValueObject
{
    public int Quantity { get; private set; }
    public override IEnumerable<object> GetEqualtyComponents()
    {
        yield return Quantity;
    }
    private Stock() { }

    private Stock(int quantity)
    {
        Quantity = quantity;
    }

    public static Stock New(int quantity)
    {
        return new(quantity);
    }
}
