using Bookstore.Domain.Enums;

namespace BookShop.Application.User.Contracts.Request;

public sealed record CreateUserRequest(string FirstName, string LastName, string Email, string PasswordHash, UserRole Role);
