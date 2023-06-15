using ChicStreetwear.Server.Options;
using ChicStreetwear.Server.Pages.Store.State.Cart;
using ChicStreetwear.Server.Services;
using ChicStreetwear.Shared.Models.Cart;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;
using Stripe;
using PaymentIntentCreateOptions = Stripe.PaymentIntentCreateOptions;

namespace ChicStreetwear.Server.Pages.Store;

public partial class Cart
{
    [Inject]
    private ISender Sender { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    [Inject]
    private CartService CartService { get; set; } = default!;

    [Inject]
    private IOptionsSnapshot<StripeOptions> StripeOptions { get; set; } = default!;

    private CartModel? Model = null;

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Model = await CartService.GetCartFromDatabaseProducts();
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task HandleOnQuantityInput(CartProductModel cartProduct, ChangeEventArgs eventArgs)
    {
        if (int.TryParse(eventArgs.Value?.ToString(), out int quantity))
        {
            await UpdateProductQuantity(cartProduct, quantity > cartProduct.MaxQuantity ? cartProduct.MaxQuantity : quantity);
        }
    }

    private async Task UpdateProductQuantity(CartProductModel cartProduct, int quantity)
    {
        var query = Model!.Products.Where(p => p.ProductId.Equals(cartProduct.ProductId));
        if (cartProduct.VariationId is not null)
        {
            query = query.Where(p => p.VariationId.Equals(cartProduct.VariationId));
        }
        var product = query.FirstOrDefault();
        if (product is not null && quantity >= 0 && quantity <= cartProduct.MaxQuantity)
        {
            Model.Total += (quantity - cartProduct.Quantity) * product.Price;
            product.Quantity = quantity;
            if (quantity == 0)
            {
                Model.Products.Remove(product);
            }
            HashSet<CartProductStateModel> stateProducts = new(Model.Products.Select(p => new CartProductStateModel(p.ProductId, p.VariationId, p.Quantity)).ToList());
            await CartService.SaveProducts(stateProducts);
            //Dispatcher.Dispatch(new SaveCartInLocalStorageAction(stateProducts));
            StateHasChanged();
        }
    }

    private void HandleCheckoutButtonClick()
    {
        StripeConfiguration.ApiKey = StripeOptions.Value.ApiKey;

        var customerService = new CustomerService();
        var customer = customerService.Create(new CustomerCreateOptions());

        var paymentIntentService = new PaymentIntentService();

        var paymentIntent = paymentIntentService.Create(new PaymentIntentCreateOptions
        {
            Customer = customer.Id,
            SetupFutureUsage = StripeOptions.Value.PaymentIntentCreateOptions.SetupFutureUsage,
            Amount = (long)Model!.Total * 100,
            Currency = StripeOptions.Value.PaymentIntentCreateOptions.Currency,
            PaymentMethodTypes = StripeOptions.Value.PaymentIntentCreateOptions.PaymentMethodTypes.ToList()
        });

        Navigation.NavigateTo($"/store/checkout/{paymentIntent.ClientSecret}");
    }
}

