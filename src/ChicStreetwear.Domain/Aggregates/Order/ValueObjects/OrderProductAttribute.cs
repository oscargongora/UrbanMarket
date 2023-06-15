using ChicStreetwear.Domain.Common;

namespace ChicStreetwear.Domain.Aggregates.Order.ValueObjects;
public sealed class OrderProductAttribute : ValueObject
{
    public string Name { get; private set; }
    public string Value { get; private set; }

    private OrderProductAttribute(string name, string value)
    {
        Name = name;
        Value = value;
    }

    public static OrderProductAttribute New(string name, string value)
    {
        return new(name, value);
    }

    public override IEnumerable<object> GetEqualtyComponents()
    {
        yield return Name;
        yield return Value;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private OrderProductAttribute() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
