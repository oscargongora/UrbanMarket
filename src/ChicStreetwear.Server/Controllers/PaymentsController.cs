using ChicStreetwear.Server.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace ChicStreetwear.Server.Controllers;

public class PaymentsController : ApiControllerBase
{
    private readonly IOptionsMonitor<StripeOptions> _stripeOptions;

    public PaymentsController(IOptionsMonitor<StripeOptions> stripeOptions)
    {
        _stripeOptions = stripeOptions;
    }

    [AllowAnonymous]
    [HttpPost]
    //stripe listen --forward-to https://localhost:7153/api/payments
    public async Task<IActionResult> Index()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], _stripeOptions.CurrentValue.WebhookSecret);

            // Handle the event
            if (stripeEvent.Type == Events.PaymentIntentSucceeded)
            {
            }
            else
            {
            }
            return Ok();
        }
        catch (StripeException e)
        {
            Console.WriteLine(e.Message);

            return BadRequest();
        }
    }
}
