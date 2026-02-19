namespace BookShop.Application.Publisher.Contracts.Response;

public sealed record PublisherResponse(
    int Id,
    string Name,
    string? Address,
    string? City,
    string? Country,
    string? PhoneNumber);
