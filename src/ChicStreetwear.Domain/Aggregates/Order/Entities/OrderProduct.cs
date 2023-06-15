using ChicStreetwear.Domain.Aggregates.Order.ValueObjects;
using ChicStreetwear.Domain.Common;
using ChicStreetwear.Domain.ValueObjects;

namespace ChicStreetwear.Domain.Aggregates.Order.Entities;

public sealed class OrderProduct : EntityBase
{
    private readonly List<OrderProductAttribute> _attributes = new();
    public Guid ProductId { get; private set; }
    public Guid? VariationId { get; private set; }
    public IReadOnlyList<OrderProductAttribute> Attributes => _attributes.AsReadOnly();
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Quantity { get; private set; }
    public Money Price { get; private set; }
    public Guid SellerId { get; private set; }

    private OrderProduct(Guid productId, Guid? variationId, string name, string description, int quantity, Money price, Guid sellerId)
    {
        ProductId = productId;
        VariationId = variationId;
        Name = name;
        Description = description;
        Quantity = quantity;
        Price = price;
        SellerId = sellerId;
    }

    public static OrderProduct New(Guid productId, string name, string description, int quantity, decimal price, Guid sellerId, Guid? variationId = null, List<OrderProductAttribute>? attributes = null)
    {
        OrderProduct product = new(productId, variationId, name, description, quantity, Money.New(price), sellerId);
        if (variationId is not null && attributes is not null)
        {
            product.AddAttributes(attributes);
        }
        return product;
    }

    private void AddAttributes(List<OrderProductAttribute> attributes)
    {
        foreach (var attribute in attributes)
        {
            AddAttribute(attribute);
        }
    }

    private void AddAttribute(OrderProductAttribute attribute)
    {
        _attributes.Add(attribute);
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public OrderProduct() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}