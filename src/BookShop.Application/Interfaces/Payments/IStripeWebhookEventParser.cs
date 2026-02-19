using BookShop.Application.Payment.Contracts;

namespace BookShop.Application.Interfaces.Payments;

public interface IStripeWebhookEventParser
{
    StripeWebhookEventData Parse(string payload, string signatureHeader);
}
