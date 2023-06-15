using ChicStreetwear.Application.Products.Queries;
using ChicStreetwear.Server.Pages.Store.State.Cart;
using ChicStreetwear.Shared.Models.Cart;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ChicStreetwear.Server.Services;

public sealed class CartService
{
    public static string CART_KEY = "CART";

    private readonly ISender _sender;
    private readonly IDispatcher _dispatcher;
    private readonly ProtectedLocalStorage _localStorage;

    public CartService(IDispatcher dispatcher, ProtectedLocalStorage localStorage, ISender sender)
    {
        _dispatcher = dispatcher;
        _localStorage = localStorage;
        _sender = sender;
    }

    public async Task<CartModel> GetCartFromDatabaseProducts()
    {
        var localProducts = await _localStorage.GetAsync<HashSet<CartProductStateModel>>(CART_KEY);

        List<CartProductModel> products = new();
        decimal total = 0;

        if (localProducts.Success && localProducts.Value is not null)
        {
            var productsResult = await _sender.Send(new GetCartProductsQuery()
            {
                Products = localProducts.Value.Select(p => new CartProductQueryModel(p.ProductId, p.VariationId, p.Quantity)).ToList()
            });

            if (!productsResult.HasErrors)
            {
                products = productsResult.Data.ToList();
                total = products.Sum(p => p.Price * p.Quantity);
            }
        }
        return new() { Products = products, Total = total };
    }

    public async Task LoadCartFromLocalStorage()
    {
        _dispatcher.Dispatch(new SetLoadingCartAction(true));
        var products = await _localStorage.GetAsync<HashSet<CartProductStateModel>>(CART_KEY);
        if (products.Success && products.Value is not null)
        {
            _dispatcher.Dispatch(new SetProductsCartAction(products.Value));
        }
        _dispatcher.Dispatch(new SetLoadingCartAction(false));
    }

    public async Task AddProductToCart(HashSet<CartProductStateModel> cartProducts, CartProductStateModel product)
    {
        var products = cartProducts.ToHashSet();
        products.Add(product);
        await _localStorage.SetAsync(CART_KEY, products);
        _dispatcher.Dispatch(new SetProductsCartAction(products));
    }

    public async Task RemoveProductFromCart(HashSet<CartProductStateModel> cartProducts, CartProductStateModel product)
    {
        var products = cartProducts.ToHashSet();
        products.Remove(product);
        await _localStorage.SetAsync(CART_KEY, products);
        _dispatcher.Dispatch(new SetProductsCartAction(products));
    }

    public async Task SaveProducts(HashSet<CartProductStateModel> products)
    {
        _dispatcher.Dispatch(new SetLoadingCartAction(true));
        await _localStorage.SetAsync(CART_KEY, products);
        _dispatcher.Dispatch(new SetProductsCartAction(products));
        _dispatcher.Dispatch(new SetLoadingCartAction(false));
    }

    public async Task ClearCart()
    {
        await _localStorage.DeleteAsync(CART_KEY);
        _dispatcher.Dispatch(new SetProductsCartAction(new(0)));
    }
}
