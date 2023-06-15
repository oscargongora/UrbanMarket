using ChicStreetwear.Domain.Aggregates.Cart.ValueObjects;
using ChicStreetwear.Domain.Common;
using ChicStreetwear.Domain.ValueObjects;

namespace ChicStreetwear.Domain.Aggregates.Cart.Entities;

public sealed class CartProduct : EntityBase
{
    private readonly List<CartProductAttribute> _attributes = new();
    public Guid ProductId { get; private set; }
    public Guid? VariationId { get; private set; }
    public IReadOnlyList<CartProductAttribute> Attributes => _attributes.AsReadOnly();
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Quantity { get; private set; }
    public Money Price { get; private set; }
    public Guid SellerId { get; private set; }

    private CartProduct(Guid id, Guid productId, Guid? variationId, string name, string description, int quantity, Money price, Guid sellerId) : base(id)
    {
        ProductId = productId;
        VariationId = variationId;
        Name = name;
        Description = description;
        Quantity = quantity;
        Price = price;
        SellerId = sellerId;
    }

    public static CartProduct New(Guid productId, string name, string description, int quantity, decimal price, Guid sellerId, List<CartProductAttribute>? attributes = null, Guid? variationId = null)
    {
        CartProduct product = new(Guid.NewGuid(), productId, variationId, name, description, quantity, Money.New(price), sellerId);

        if (variationId is not null && attributes is not null)
        {
            product.AddAttributes(attributes);
        }

        return product;
    }

    private void AddAttributes(List<CartProductAttribute> attributes) => _attributes.AddRange(attributes);

    public void Update(string name, string description, int quantity, decimal price)
    {
        Name = name;
        Description = description;
        Quantity = quantity;
        Price = Money.New(price);
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private CartProduct() : base() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}