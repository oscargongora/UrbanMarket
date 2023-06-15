namespace ChicStreetwear.Server.Options;

public class ShippingAddressCollectionOption
{
    public string[] AllowedCountries { get; set; }
}

public class DeliveryEstimateOption
{
    public string Unit { get; set; }
    public int Value { get; set; }
}

public class ShippingOption
{
    public int Amount { get; set; }
    public string Currency { get; set; }
    public string DisplayName { get; set; }
    public DeliveryEstimateOption MinimumDeliveryEstimate { get; set; }
    public DeliveryEstimateOption MaximumDeliveryEstimate { get; set; }
}

public class SessionCreateOptions
{
    public ShippingOption[] ShippingOptions { get; set; }
    public string[] PaymentMethodTypes { get; set; }
    public ShippingAddressCollectionOption ShippingAddressCollection { get; set; }
    public string BillingAddressCollection { get; set; }
    public string SuccessUrl { get; set; }
    public string CancelUrl { get; set; }
}

public class PaymentIntentCreateOptions
{
    public string SetupFutureUsage { get; set; }
    public string Currency { get; set; }
    public string[] PaymentMethodTypes { get; set; }
    public string SuccessUrl { get; set; }
}

public class StripeOptions
{
    public static string SECTION_NAME = "StripeConfiguration";
    public string ApiKey { get; set; }
    public string PublishableKey { get; set; }
    public string WebhookSecret { get; set; }
    public PaymentIntentCreateOptions PaymentIntentCreateOptions { get; set; }
}
