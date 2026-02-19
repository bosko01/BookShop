namespace BookShop.Application.Review.Contracts.Request;

public sealed record UpdateReviewRequest(int Rating, string? Comment);
