namespace ChicStreetwear.Domain.Common;

public abstract class ValueObject : IEquatable<ValueObject>
{
    public abstract IEnumerable<object> GetEqualtyComponents();

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        var valueObject = (ValueObject)obj;

        return GetEqualtyComponents().SequenceEqual(valueObject.GetEqualtyComponents());
    }

    public static bool operator ==(ValueObject valueObject1, ValueObject valueObject2)
    {
        return Equals(valueObject1, valueObject2);
    }

    public static bool operator !=(ValueObject valueObject1, ValueObject valueObject2)
    {
        return !Equals(valueObject1, valueObject2);
    }

    public override int GetHashCode()
    {
        return GetEqualtyComponents()
        .Select(eqComp => eqComp?.GetHashCode() ?? 0)
        .Aggregate((hashCode1, hashCode2) => hashCode1 ^ hashCode2);
    }

    public bool Equals(ValueObject? other)
    {
        return Equals((object?)other);
    }
}