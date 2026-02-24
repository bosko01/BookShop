namespace BookShop.Application.Payment.Contracts;

public sealed record CreateStripeCheckoutSessionRequest(int OrderId, decimal Amount, string Currency, string SuccessUrl, string CancelUrl);
