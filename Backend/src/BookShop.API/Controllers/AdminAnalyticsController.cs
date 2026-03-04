using BookShop.Application.AdminAnalytics.Contracts.Response;
using BookShop.Application.AdminAnalytics.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.API.Controllers;

[ApiController]
[Route("api/admin/analytics")]
[Authorize(Roles = "Admin")]
public sealed class AdminAnalyticsController : ControllerBase
{
    private readonly IAdminAnalyticsService _adminAnalyticsService;

    public AdminAnalyticsController(IAdminAnalyticsService adminAnalyticsService)
    {
        _adminAnalyticsService = adminAnalyticsService;
    }

    [HttpGet]
    public async Task<ActionResult<AdminAnalyticsResponse>> Get(CancellationToken cancellationToken)
    {
        var response = await _adminAnalyticsService.GetAsync(cancellationToken);
        return Ok(response);
    }
}
