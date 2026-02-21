namespace BookShop.Application.Review.Contracts.Response;

public sealed record ReviewResponse(int Id, int BookId, int UserId, int Rating, string? Comment);
