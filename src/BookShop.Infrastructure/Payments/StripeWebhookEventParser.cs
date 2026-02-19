using BookShop.Application.Interfaces.Payments;
using BookShop.Application.Payment.Contracts;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace BookShop.Infrastructure.Payments;

public sealed class StripeWebhookEventParser : IStripeWebhookEventParser
{
    private readonly string _webhookSecret;

    public StripeWebhookEventParser(IConfiguration configuration)
    {
        _webhookSecret = configuration["Stripe:WebhookSecret"]
            ?? throw new InvalidOperationException("Stripe:WebhookSecret is not configured.");
    }

    public StripeWebhookEventData Parse(string payload, string signatureHeader)
    {
        var stripeEvent = EventUtility.ConstructEvent(payload, signatureHeader, _webhookSecret);

        return stripeEvent.Type switch
        {
            Events.PaymentIntentSucceeded => new StripeWebhookEventData(true, TryGetOrderId((stripeEvent.Data.Object as PaymentIntent)?.Metadata)),
            Events.CheckoutSessionCompleted => new StripeWebhookEventData(true, GetCheckoutOrderId(stripeEvent.Data.Object as Session)),
            _ => new StripeWebhookEventData(false, null)
        };
    }

    private static int? GetCheckoutOrderId(Session? session)
    {
        var fromMetadata = TryGetOrderId(session?.Metadata);
        if (fromMetadata is not null)
            return fromMetadata;

        if (int.TryParse(session?.ClientReferenceId, out var orderIdFromClientReference))
            return orderIdFromClientReference;

        return null;
    }

    private static int? TryGetOrderId(IDictionary<string, string>? metadata)
    {
        if (metadata is null)
            return null;

        if (metadata.TryGetValue("orderId", out var orderIdValue) && int.TryParse(orderIdValue, out var orderId))
            return orderId;

        if (metadata.TryGetValue("order_id", out var snakeOrderIdValue) && int.TryParse(snakeOrderIdValue, out var snakeOrderId))
            return snakeOrderId;

        return null;
    }
}
