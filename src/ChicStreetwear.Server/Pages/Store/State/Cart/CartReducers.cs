using Fluxor;

namespace ChicStreetwear.Server.Pages.Store.State.Cart;

public static class CartReducers
{
    [ReducerMethod]
    public static CartState ReduceSetLoadingCartAction(CartState state, SetLoadingCartAction action)
    {
        return new(action.Loading, state.Products);
    }

    [ReducerMethod]
    public static CartState ReduceSetProductsInCartAction(CartState state, SetProductsCartAction action)
    {
        return new(state.Loading, action.Products);
    }
}
