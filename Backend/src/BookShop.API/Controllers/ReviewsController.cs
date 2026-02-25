using System.Security.Claims;
using BookShop.Application.Review.Contracts;
using BookShop.Application.Review.Contracts.Request;
using BookShop.Application.Review.Contracts.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;
    public ReviewsController(IReviewService reviewService) => _reviewService = reviewService;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ReviewResponse>>> GetAll(CancellationToken ct) => Ok(await _reviewService.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReviewResponse>> GetById(int id, CancellationToken ct) => Ok(await _reviewService.GetByIdAsync(id, ct));

    [HttpGet("by-book/{bookId:int}")]
    public async Task<ActionResult<IReadOnlyList<ReviewResponse>>> GetByBook(int bookId, CancellationToken ct) => Ok(await _reviewService.GetByBookIdAsync(bookId, ct));

    [HttpGet("by-user/{userId:int}")]
    public async Task<ActionResult<IReadOnlyList<ReviewResponse>>> GetByUser(int userId, CancellationToken ct) => Ok(await _reviewService.GetByUserIdAsync(userId, ct));

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<int>> Create(
    [FromBody] CreateReviewRequest request,
    CancellationToken ct)
    {
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var newRequest = request with { UserId = userId };

        var id = await _reviewService.CreateAsync(newRequest, ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<ActionResult<ReviewResponse>> Update(int id, [FromBody] UpdateReviewRequest request, CancellationToken ct)
        => Ok(await _reviewService.UpdateAsync(id, request, ct));

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _reviewService.DeleteAsync(id, ct);
        return NoContent();
    }
}
