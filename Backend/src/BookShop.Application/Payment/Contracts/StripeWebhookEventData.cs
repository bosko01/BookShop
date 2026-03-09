namespace BookShop.Application.Payment.Contracts;

public sealed record StripeWebhookEventData(string EventType, bool IsCheckoutSessionCompleted, int? OrderId, string? ProviderReference);
