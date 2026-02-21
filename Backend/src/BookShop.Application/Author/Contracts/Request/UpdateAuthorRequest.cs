namespace BookShop.Application.Author.Contracts.Request;

public sealed record UpdateAuthorRequest(
    int Id,
    string FirstName,
    string LastName,
    string? Biography
);
