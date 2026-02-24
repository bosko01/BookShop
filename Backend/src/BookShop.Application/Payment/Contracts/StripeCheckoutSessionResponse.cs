namespace BookShop.Application.Payment.Contracts;

public sealed record StripeCheckoutSessionResponse(string SessionId, string Url);
