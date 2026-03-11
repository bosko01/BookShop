namespace BookShop.Application.Publisher.Contracts.Response;

public sealed record PublisherListResponse(
    int Id,
    string Name,
    string? Address,
    string? City,
    string? Country,
    string? PhoneNumber);
