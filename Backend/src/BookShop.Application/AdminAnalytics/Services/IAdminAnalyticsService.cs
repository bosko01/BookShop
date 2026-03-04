using BookShop.Application.AdminAnalytics.Contracts.Response;

namespace BookShop.Application.AdminAnalytics.Services;

public interface IAdminAnalyticsService
{
    Task<AdminAnalyticsResponse> GetAsync(CancellationToken cancellationToken = default);
}
