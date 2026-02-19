using BookShop.Application.Author.Contracts.Request;
using BookShop.Application.Author.Services;
using BookShop.Application.Author.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<AuthorResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<AuthorResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var authors = await _authorService.GetAllAsync(cancellationToken);
        return Ok(authors);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AuthorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuthorResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var author = await _authorService.GetByIdAsync(id, cancellationToken);
        if (author is null)
        {
            return NotFound();
        }

        return Ok(author);
    }

    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    public async Task<ActionResult> Create([FromBody] CreateAuthorRequest request, CancellationToken cancellationToken)
    {
        var id = await _authorService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateAuthorRequest request, CancellationToken cancellationToken)
    {
        await _authorService.UpdateAsync(id, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _authorService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
