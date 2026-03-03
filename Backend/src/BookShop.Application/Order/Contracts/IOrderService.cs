using BookShop.Application.Order.Contracts.Request;
using BookShop.Application.Order.Contracts.Response;

namespace BookShop.Application.Order.Contracts;

public interface IOrderService
{
    Task<IReadOnlyList<OrderResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrderResponse>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<OrderResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<OrderResponse?> GetByIdForUserAsync(int id, int userId, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task MarkAsPaidAsync(int id, CancellationToken cancellationToken = default);
    Task MarkAsShippedAsync(int id, CancellationToken cancellationToken = default);
    Task MarkAsDeliveredAsync(int id, CancellationToken cancellationToken = default);
    Task CancelAsync(int id, CancellationToken cancellationToken = default);
    Task ChangeItemQuantityAsync(int id, ChangeOrderItemQuantityRequest request, CancellationToken cancellationToken = default);
}
