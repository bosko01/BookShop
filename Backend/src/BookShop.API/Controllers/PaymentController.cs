using System.Security.Claims;
using System.Text;
using BookShop.Application.Interfaces.Payments;
using BookShop.Application.Payment.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class PaymentController : ControllerBase
{
    private readonly IPaymentWebhookService _paymentWebhookService;
    private readonly IStripeCheckoutService _stripeCheckoutService;

    public PaymentController(IPaymentWebhookService paymentWebhookService, IStripeCheckoutService stripeCheckoutService)
    {
        _paymentWebhookService = paymentWebhookService;
        _stripeCheckoutService = stripeCheckoutService;
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
        using var reader = new StreamReader(Request.Body, Encoding.UTF8);
        var payload = await reader.ReadToEndAsync(cancellationToken);
        var signature = Request.Headers["Stripe-Signature"].ToString();

        await _paymentWebhookService.HandleStripeWebhookAsync(payload, signature, cancellationToken);
        return Ok();
    }
}

public sealed record CreateCheckoutSessionPayload(int OrderId, int UserId, decimal Amount, string Currency, string SuccessUrl, string CancelUrl);
