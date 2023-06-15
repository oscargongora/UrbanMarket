using ChicStreetwear.Domain.Common;

namespace ChicStreetwear.Domain.Aggregates.Product.Entities;

public sealed class AttributeVariation : EntityBase
{
    public string Value { get; private set; }
    public ValueObjects.Attribute Attribute { get; private set; }

    private AttributeVariation(Guid id, string value, ValueObjects.Attribute attribute) : base(id)
    {
        Value = value;
        Attribute = attribute;
    }

    public static AttributeVariation New(string value, ValueObjects.Attribute attribute)
    {
        return new(Guid.NewGuid(), value, attribute);
    }

    public void Update(string value)
    {
        Value = value;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private AttributeVariation() : base() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}