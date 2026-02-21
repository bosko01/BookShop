using Bookstore.Domain.Enums;

namespace BookShop.Application.User.Contracts.Request;

public sealed record SetUserRoleRequest(UserRole Role);
