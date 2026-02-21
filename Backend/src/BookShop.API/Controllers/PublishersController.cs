using BookShop.Application.Publisher.Contracts;
using BookShop.Application.Publisher.Contracts.Request;
using BookShop.Application.Publisher.Contracts.Response;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class PublishersController : ControllerBase
{
    private readonly IPublisherService _publisherService;
    public PublishersController(IPublisherService publisherService) => _publisherService = publisherService;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PublisherListResponse>>> GetAll(CancellationToken ct) => Ok(await _publisherService.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PublisherResponse>> GetById(int id, CancellationToken ct) => Ok(await _publisherService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreatePublisherRequest request, CancellationToken ct)
    {
        var id = await _publisherService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PublisherResponse>> Update(int id, [FromBody] UpdatePublisherRequest request, CancellationToken ct)
        => Ok(await _publisherService.UpdateAsync(id, request, ct));

    [HttpPatch("{id:int}/contact")]
    public async Task<ActionResult<PublisherResponse>> UpdateContact(int id, [FromBody] UpdatePublisherContactRequest request, CancellationToken ct)
        => Ok(await _publisherService.UpdateContactAsync(id, request, ct));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _publisherService.DeleteAsync(id, ct);
        return NoContent();
    }
}
