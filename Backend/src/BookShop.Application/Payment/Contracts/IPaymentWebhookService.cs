namespace BookShop.Application.Payment.Contracts;

public interface IPaymentWebhookService
{
    Task HandleStripeWebhookAsync(string payload, string signatureHeader, CancellationToken cancellationToken = default);
}
