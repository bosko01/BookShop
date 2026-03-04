namespace BookShop.Application.AdminAnalytics.Contracts.Response;

public sealed record AdminAnalyticsResponse(
    decimal TotalSales,
    int Orders,
    int Books,
    int Customers);
