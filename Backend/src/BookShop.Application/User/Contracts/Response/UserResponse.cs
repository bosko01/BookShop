using Bookstore.Domain.Enums;

namespace BookShop.Application.User.Contracts.Response;

public sealed record UserResponse(int Id, string FirstName, string LastName, string Email, UserRole Role, DateTime CreatedAtUtc);
