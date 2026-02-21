using BookShop.Application.PaymentMethod.Contracts.Request;
using BookShop.Application.PaymentMethod.Contracts.Response;

namespace BookShop.Application.PaymentMethod.Contracts;

public interface IPaymentMethodService
{
    Task<IReadOnlyList<PaymentMethodResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PaymentMethodResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(CreatePaymentMethodRequest request, CancellationToken cancellationToken = default);
    Task<PaymentMethodResponse> UpdateAsync(int id, UpdatePaymentMethodRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task ActivateAsync(int id, CancellationToken cancellationToken = default);
    Task DeactivateAsync(int id, CancellationToken cancellationToken = default);
}
