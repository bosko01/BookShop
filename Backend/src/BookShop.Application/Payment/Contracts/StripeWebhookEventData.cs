namespace BookShop.Application.Payment.Contracts;

public sealed record StripeWebhookEventData(bool IsSuccessfulPaymentEvent, int? OrderId);
