using System.Security.Claims;
using System.Text;
using BookShop.Application.Interfaces.Payments;
using BookShop.Application.Payment.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class PaymentController : ControllerBase
{
    private readonly IPaymentWebhookService _paymentWebhookService;
    private readonly IStripeCheckoutService _stripeCheckoutService;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(IPaymentWebhookService paymentWebhookService, IStripeCheckoutService stripeCheckoutService, ILogger<PaymentController> logger)
    {
        _paymentWebhookService = paymentWebhookService;
        _stripeCheckoutService = stripeCheckoutService;
        _logger = logger;
    }

    [HttpPost("checkout-session")]
    [Authorize]
    public async Task<ActionResult<StripeCheckoutSessionResponse>> CreateCheckoutSession([FromBody] CreateCheckoutSessionPayload payload, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var userIdFromToken) || userIdFromToken != payload.UserId)
            return Forbid();

        var response = await _stripeCheckoutService.CreateCheckoutSessionAsync(
            new CreateStripeCheckoutSessionRequest(payload.OrderId, payload.Amount, payload.Currency, payload.SuccessUrl, payload.CancelUrl),
            cancellationToken);

        return Ok(response);
    }

    [HttpPost("webhook")]
    [AllowAnonymous]
    public async Task<IActionResult> StripeWebhook(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stripe webhook request received. Path={Path}", Request.Path);

        using var reader = new StreamReader(Request.Body, Encoding.UTF8);
        var payload = await reader.ReadToEndAsync(cancellationToken);
        var signature = Request.Headers["Stripe-Signature"].ToString();

        await _paymentWebhookService.HandleStripeWebhookAsync(payload, signature, cancellationToken);

        _logger.LogInformation("Stripe webhook response returned with HTTP 200.");
        return Ok();
    }
}

public sealed record CreateCheckoutSessionPayload(int OrderId, int UserId, decimal Amount, string Currency, string SuccessUrl, string CancelUrl);
