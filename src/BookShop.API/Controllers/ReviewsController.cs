using BookShop.Application.Review.Contracts;
using BookShop.Application.Review.Contracts.Request;
using BookShop.Application.Review.Contracts.Response;
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
    public async Task<ActionResult<int>> Create([FromBody] CreateReviewRequest request, CancellationToken ct)
    {
        var id = await _reviewService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ReviewResponse>> Update(int id, [FromBody] UpdateReviewRequest request, CancellationToken ct)
        => Ok(await _reviewService.UpdateAsync(id, request, ct));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _reviewService.DeleteAsync(id, ct);
        return NoContent();
    }
}
