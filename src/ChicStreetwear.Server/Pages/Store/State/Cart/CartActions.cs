namespace ChicStreetwear.Server.Pages.Store.State.Cart;

public sealed record SetLoadingCartAction(bool Loading);
public sealed record SetProductsCartAction(HashSet<CartProductStateModel> Products);

