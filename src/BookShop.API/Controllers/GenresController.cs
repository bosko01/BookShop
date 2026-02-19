using BookShop.Application.Genre.Contracts;
using BookShop.Application.Genre.Contracts.Request;
using BookShop.Application.Genre.Contracts.Response;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class GenresController : ControllerBase
{
    private readonly IGenreService _genreService;
    public GenresController(IGenreService genreService) => _genreService = genreService;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GenreResponse>>> GetAll(CancellationToken ct) => Ok(await _genreService.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GenreResponse>> GetById(int id, CancellationToken ct) => Ok(await _genreService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateGenreRequest request, CancellationToken ct)
    {
        var id = await _genreService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<GenreResponse>> Update(int id, [FromBody] UpdateGenreRequest request, CancellationToken ct)
        => Ok(await _genreService.UpdateAsync(id, request, ct));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _genreService.DeleteAsync(id, ct);
        return NoContent();
    }
}
