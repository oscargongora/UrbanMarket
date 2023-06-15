using ChicStreetwear.Domain.Common;

namespace ChicStreetwear.Domain.ValueObjects;

public class Rating : ValueObject
{
    public float Value { get; private set; }
    public int Count { get; private set; }

    private Rating(float value, int count)
    {
        Value = value;
        Count = count;
    }

    public static Rating New(float value = 0, int count = 0)
    {
        return new Rating(value, count);
    }

    public override IEnumerable<object> GetEqualtyComponents()
    {
        yield return Value;
    }
}

