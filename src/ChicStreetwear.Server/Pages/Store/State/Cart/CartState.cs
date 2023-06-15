using Fluxor;

namespace ChicStreetwear.Server.Pages.Store.State.Cart;

public sealed class CartProductStateModel
{
    public Guid ProductId { get; set; }
    public Guid? VariationId { get; set; }
    public int Quantity { get; set; }

    public CartProductStateModel(Guid productId, Guid? variationId, int quantity)
    {
        ProductId = productId;
        VariationId = variationId;
        Quantity = quantity;
    }

    public CartProductStateModel(Guid productId, Guid? variationId)
    {
        ProductId = productId;
        VariationId = variationId;
        Quantity = 0;
    }

    public CartProductStateModel() { }

    public override bool Equals(object? obj)
    {
        return obj is CartProductStateModel model &&
               ProductId.Equals(model.ProductId) &&
               EqualityComparer<Guid?>.Default.Equals(VariationId, model.VariationId);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ProductId, VariationId);
    }
}

[FeatureState]
public sealed class CartState
{
    public bool Loading { get; }
    public HashSet<CartProductStateModel> Products { get; }

    public CartState(bool loading, HashSet<CartProductStateModel> products)
    {
        Loading = loading;
        Products = products;
    }

    private CartState()
    {
        Loading = true;
        Products = new();
    }
}
