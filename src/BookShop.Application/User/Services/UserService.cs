using BookShop.Application.Common.Exceptions;
using BookShop.Application.Interfaces.Persistence;
using BookShop.Application.Interfaces.Persistence.Common;
using BookShop.Application.User.Contracts;
using BookShop.Application.User.Contracts.Request;
using BookShop.Application.User.Contracts.Response;

namespace BookShop.Application.User.Services;

public sealed class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        => (await _userRepository.GetAllAsync(cancellationToken)).Select(Map).ToList();

    public async Task<UserResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("User", id);
        return Map(user);
    }

    public async Task<UserResponse> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken) ?? throw new NotFoundException($"User with email '{email}' was not found.");
        return Map(user);
    }

    public async Task<int> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        if (await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            throw new ConflictException("User with this email already exists.");

        var user = Domain.Entities.User.Create(request.FirstName, request.LastName, request.Email, request.PasswordHash, request.Role);
        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        return user.Id;
    }

    public async Task<UserResponse> UpdateAsync(int id, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("User", id);

        if (!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase) &&
            await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            throw new ConflictException("User with this email already exists.");

        user.UpdateProfile(request.FirstName, request.LastName, request.Email);

        await _unitOfWork.SaveAsync(cancellationToken);
        return Map(user);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("User", id);
        user.MarkAsDeleted(DateTime.UtcNow);
        await _unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task ChangePasswordAsync(int id, ChangeUserPasswordRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("User", id);
        user.ChangePassword(request.PasswordHash);
        await _unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task SetRoleAsync(int id, SetUserRoleRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("User", id);
        user.SetRole(request.Role);
        await _unitOfWork.SaveAsync(cancellationToken);
    }

    private static UserResponse Map(Domain.Entities.User user) => new(user.Id, user.FirstName, user.LastName, user.Email, user.Role, user.CreatedAtUtc);
}
