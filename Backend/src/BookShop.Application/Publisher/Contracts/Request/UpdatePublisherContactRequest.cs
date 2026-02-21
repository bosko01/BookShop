namespace BookShop.Application.Publisher.Contracts.Request;

public sealed record UpdatePublisherContactRequest(
    string? Address,
    string? City,
    string? Country,
    string? PhoneNumber);
