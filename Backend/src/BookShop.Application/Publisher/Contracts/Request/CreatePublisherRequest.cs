namespace BookShop.Application.Publisher.Contracts.Request;

public sealed record CreatePublisherRequest(
    string Name,
    string? Address,
    string? City,
    string? Country,
    string? PhoneNumber);
