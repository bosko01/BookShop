using BookShop.Application.Interfaces.Payments;
using BookShop.Application.Payment.Contracts;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;

namespace BookShop.Infrastructure.Payments;

public sealed class StripeCheckoutService : IStripeCheckoutService
{
    private static readonly HashSet<string> ZeroDecimalCurrencies = new(StringComparer.OrdinalIgnoreCase)
    {
        "bif", "clp", "djf", "gnf", "jpy", "kmf", "krw", "mga", "pyg", "rwf", "ugx", "vnd", "vuv", "xaf", "xof", "xpf"
    };

    private readonly SessionService _sessionService;

    public StripeCheckoutService(IConfiguration configuration)
    {
        var apiKey = configuration["Stripe:SecretKey"] ?? throw new InvalidOperationException("Stripe:SecretKey is not configured.");
        StripeConfiguration.ApiKey = apiKey;
        _sessionService = new SessionService();
    }

    public async Task<StripeCheckoutSessionResponse> CreateCheckoutSessionAsync(CreateStripeCheckoutSessionRequest request, CancellationToken cancellationToken = default)
    {
        var options = new SessionCreateOptions
        {
            Mode = "payment",
            SuccessUrl = request.SuccessUrl,
            CancelUrl = request.CancelUrl,
            ClientReferenceId = request.OrderId.ToString(),
            Metadata = new Dictionary<string, string>
            {
                ["orderId"] = request.OrderId.ToString()
            },
            LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    Quantity = 1,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = request.Currency.ToLowerInvariant(),
                        UnitAmount = ConvertAmountToMinorUnit(request.Amount, request.Currency),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"BookShop order #{request.OrderId}"
                        }
                    }
                }
            },
            PaymentIntentData = new SessionPaymentIntentDataOptions
            {
                Metadata = new Dictionary<string, string>
                {
                    ["orderId"] = request.OrderId.ToString()
                }
            }
        };

        var session = await _sessionService.CreateAsync(options, cancellationToken: cancellationToken);
        return new StripeCheckoutSessionResponse(session.Id, session.Url ?? string.Empty);
    }

    private static long ConvertAmountToMinorUnit(decimal amount, string currency)
    {
        if (ZeroDecimalCurrencies.Contains(currency))
            return (long)Math.Round(amount, MidpointRounding.AwayFromZero);

        return (long)Math.Round(amount * 100, MidpointRounding.AwayFromZero);
    }
}
