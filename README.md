<h1>Urban Market</h1>
<p>Ecommerce for the sale of fashion items. It allows the registration of users and that they add their products.</p>

<h2>Landing page</h2>

![Alt text](https://github.com/oscargongora/urbanmarket/blob/main/captures/landing_page.gif)

<h2>Hero section</h2>

![Alt text](https://github.com/oscargongora/urbanmarket/blob/main/captures/hero_section.gif)

<h2>Products page</h2>

![Alt text](https://github.com/oscargongora/urbanmarket/blob/main/captures/products_page.gif)

<h2>Cart page</h2>

![Alt text](https://github.com/oscargongora/urbanmarket/blob/main/captures/cart_page.gif)

<h2>Management system</h2>

![Alt text](https://github.com/oscargongora/urbanmarket/blob/main/captures/manage_products.gif)

<h3>Configuration<h3>

```json
{
  "ConnectionStrings": {
    "ChicStreetwearDb": "",
    "ChicStreetwearIdentityDb": ""
  },
  "Identity": {
    "DefaultAdministratorUser": {
      "UserName": "",
      "Email": "",
      "Password": ""
    }
  },
  "CloudStorage": {
    "Azure": {
      "ConnectionString": "",
      "ContainerName": ""
    }
  },
  "StripeConfiguration": {
    "ApiKey": "",
    "PublishableKey": "",
    "WebhookSecret": "",
    "PaymentIntentCreateOptions": {
      "SetupFutureUsage": "",
      "Currency": "",
      "PaymentMethodTypes:0": "",
      "SuccessUrl": "https://localhost:7153/store/checkout-success" //required
    }
  }
}
```

