using BookShop.Application.User.Contracts.Request;
using BookShop.Application.User.Contracts.Response;

namespace BookShop.Application.User.Contracts;

public interface IUserService
{
    Task<IReadOnlyList<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<UserResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<UserResponse> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
    Task<UserResponse> UpdateAsync(int id, UpdateUserRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task ChangePasswordAsync(int id, ChangeUserPasswordRequest request, CancellationToken cancellationToken = default);
    Task SetRoleAsync(int id, SetUserRoleRequest request, CancellationToken cancellationToken = default);
}
