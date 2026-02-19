namespace BookShop.Application.Review.Contracts.Request;

public sealed record CreateReviewRequest(int BookId, int UserId, int Rating, string? Comment);
