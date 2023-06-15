using ChicStreetwear.Domain.Common;

namespace ChicStreetwear.Domain.ValueObjects;

public sealed class Money : ValueObject
{
    public decimal Amount { get; private set; }
    private Money(decimal amount)
    {
        Amount = amount;
    }

    public static Money New(decimal amount)
    {
        return new(amount);
    }

    public override IEnumerable<object> GetEqualtyComponents()
    {
        yield return Amount;
    }
}

