namespace ChicStreetwear.Shared.Models.Cart;
public class CartModel
{
    public List<CartProductModel> Products { get; set; } = new();

    public decimal Total { get; set; } = 0;
}
