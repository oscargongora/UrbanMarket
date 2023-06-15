using ChicStreetwear.Domain.Aggregates.Cart;
using ChicStreetwear.Domain.Aggregates.Cart.Entities;
using ChicStreetwear.Domain.Aggregates.Cart.ValueObjects;

namespace ChicStreetwear.Application.Carts;

public sealed class CartResponse
{
    public Guid Id { get; set; }
    public List<CartProductResponse> Products { get; set; } = new();
    public decimal Total { get; set; }
    public Guid? CustomerId { get; set; }

    public static CartResponse FromCart(Cart cart)
    {
        return new()
        {
            Id = cart.Id,
            Products = cart.Products.ToList().ConvertAll(cp => CartProductResponse.FromCartProduct(cp)),
            Total = cart.Total.Amount,
            CustomerId = cart.CustomerId
        };
    }
}

public sealed class CartProductResponse
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; set; }
    public Guid? VariationId { get; set; }
    public List<CartProductAttributeResponseBase> Attributes { get; set; } = new();
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public Guid SellerId { get; set; }

    internal static CartProductResponse FromCartProduct(CartProduct cp)
    {
        return new()
        {
            Id = cp.Id,
            Name = cp.Name,
            Description = cp.Description,
            Attributes = cp.Attributes.ToList().ConvertAll(cpa => CartProductAttributeResponseBase.FromCartProductAttribute(cpa)),
            Quantity = cp.Quantity,
            Price = cp.Price.Amount,
            ProductId = cp.ProductId,
            VariationId = cp.VariationId,
            SellerId = cp.SellerId
        };
    }
}

public sealed class CartProductAttributeResponseBase
{
    public required string Name { get; set; }
    public required string Value { get; set; }

    internal static CartProductAttributeResponseBase FromCartProductAttribute(CartProductAttribute cpa)
    {
        return new() { Name = cpa.Name, Value = cpa.Value };
    }
}