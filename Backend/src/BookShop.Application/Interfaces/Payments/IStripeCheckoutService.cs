using BookShop.Application.Payment.Contracts;

namespace BookShop.Application.Interfaces.Payments;

public interface IStripeCheckoutService
{
    Task<StripeCheckoutSessionResponse> CreateCheckoutSessionAsync(CreateStripeCheckoutSessionRequest request, CancellationToken cancellationToken = default);
}
