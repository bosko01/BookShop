namespace BookShop.Application.User.Contracts.Request;

public sealed record UpdateUserRequest(string FirstName, string LastName, string Email);
