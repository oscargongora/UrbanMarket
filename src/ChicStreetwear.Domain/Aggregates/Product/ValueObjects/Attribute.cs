using ChicStreetwear.Domain.Common;

namespace ChicStreetwear.Domain.Aggregates.Product.ValueObjects;

public sealed class Attribute : ValueObject
{
    public string Name { get; private set; }
    private Attribute(string name)
    {
        Name = name;
    }

    public static Attribute New(string name)
    {
        return new(name);
    }

    public override IEnumerable<object> GetEqualtyComponents()
    {
        yield return Name;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Attribute() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}