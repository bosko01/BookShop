using BookShop.Application.Book.Contracts;
using BookShop.Application.Book.Contracts.Request;
using BookShop.Application.Book.Contracts.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    // =========================
    // GET BY ID
    // =========================

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var book = await _bookService.GetByIdAsync(id, cancellationToken);
        return Ok(book);
    }

    // =========================
    // GET ALL
    // =========================

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BookListResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var books = await _bookService.GetAllAsync(cancellationToken);
        return Ok(books);
    }

    // =========================
    // FILTERS
    // =========================

    [HttpGet("by-author/{authorId:int}")]
    public async Task<ActionResult<IReadOnlyList<BookListResponse>>> GetByAuthor(
        int authorId,
        CancellationToken cancellationToken)
    {
        var books = await _bookService.GetByAuthorIdAsync(authorId, cancellationToken);
        return Ok(books);
    }

    [HttpGet("by-genre")]
    public async Task<ActionResult<IReadOnlyList<BookListResponse>>> GetByGenre(
        [FromQuery] string genre,
        CancellationToken cancellationToken)
    {
        var books = await _bookService.GetByGenreAsync(genre, cancellationToken);
        return Ok(books);
    }

    [HttpGet("by-publisher")]
    public async Task<ActionResult<IReadOnlyList<BookListResponse>>> GetByPublisher(
        [FromQuery] string publisher,
        CancellationToken cancellationToken)
    {
        var books = await _bookService.GetByPublisherAsync(publisher, cancellationToken);
        return Ok(books);
    }

    [HttpGet("low-stock")]
    public async Task<ActionResult<IReadOnlyList<BookListResponse>>> GetLowStock(
        [FromQuery] int threshold,
        CancellationToken cancellationToken)
    {
        var books = await _bookService.GetLowStockAsync(threshold, cancellationToken);
        return Ok(books);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IReadOnlyList<BookListResponse>>> Search(
        [FromQuery] string keyword,
        CancellationToken cancellationToken)
    {
        var books = await _bookService.SearchByTitleAsync(keyword, cancellationToken);
        return Ok(books);
    }

    // =========================
    // CREATE
    // =========================

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<int>> Create(
        [FromBody] CreateBookRequest request,
        CancellationToken cancellationToken)
    {
        var id = await _bookService.CreateAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id },
            id);
    }

    // =========================
    // UPDATE
    // =========================

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<BookResponse>> Update(
        int id,
        [FromBody] UpdateBookRequest request,
        CancellationToken cancellationToken)
    {
        var updated = await _bookService.UpdateAsync(id, request, cancellationToken);
        return Ok(updated);
    }

    // =========================
    // SOFT DELETE
    // =========================

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _bookService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    // =========================
    // RESTORE
    // =========================

    [HttpPatch("{id:int}/restore")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Restore(int id, CancellationToken cancellationToken)
    {
        await _bookService.RestoreAsync(id, cancellationToken);
        return NoContent();
    }

    // =========================
    // DECREMENT STOCK
    // =========================

    [HttpPatch("{id:int}/decrement-stock")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DecrementStock(
        int id,
        [FromQuery] int amount,
        CancellationToken cancellationToken)
    {
        await _bookService.DecrementStockAsync(id, amount, cancellationToken);
        return NoContent();
    }
}
