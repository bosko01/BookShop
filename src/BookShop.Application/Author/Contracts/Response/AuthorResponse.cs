namespace BookShop.Application.Author.Contracts.Request;

public sealed record AuthorResponse(
	string FirstName,
	string LastName,
	string? Biography
);