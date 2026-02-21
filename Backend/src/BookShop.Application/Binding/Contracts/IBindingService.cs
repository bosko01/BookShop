using BookShop.Application.Binding.Contracts.Request;
using BookShop.Application.Binding.Contracts.Response;

namespace BookShop.Application.Binding.Contracts;

public interface IBindingService
{
    Task<IReadOnlyList<BindingResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<BindingResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(CreateBindingRequest request, CancellationToken cancellationToken = default);
    Task<BindingResponse> UpdateAsync(int id, UpdateBindingRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
