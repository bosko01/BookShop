using System.Text;
using BookShop.Application.Payment.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class PaymentController : ControllerBase
{
    private readonly IPaymentWebhookService _paymentWebhookService;

    public PaymentController(IPaymentWebhookService paymentWebhookService)
        => _paymentWebhookService = paymentWebhookService;

    [HttpPost("webhook")]
    [AllowAnonymous]
    public async Task<IActionResult> StripeWebhook(CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(Request.Body, Encoding.UTF8);
        var payload = await reader.ReadToEndAsync(cancellationToken);
        var signature = Request.Headers["Stripe-Signature"].ToString();

        await _paymentWebhookService.HandleStripeWebhookAsync(payload, signature, cancellationToken);
        return Ok();
    }
}
