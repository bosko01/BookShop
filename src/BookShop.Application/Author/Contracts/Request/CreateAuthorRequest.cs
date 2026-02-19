namespace BookShop.Application.Author.Contracts.Request;
public sealed record CreateAuthorRequest(
    string FirstName,
    string LastName,
    string? Biography
);