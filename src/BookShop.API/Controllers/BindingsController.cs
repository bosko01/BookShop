using BookShop.Application.Binding.Contracts;
using BookShop.Application.Binding.Contracts.Request;
using BookShop.Application.Binding.Contracts.Response;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class BindingsController : ControllerBase
{
    private readonly IBindingService _bindingService;
    public BindingsController(IBindingService bindingService) => _bindingService = bindingService;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BindingResponse>>> GetAll(CancellationToken ct) => Ok(await _bindingService.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BindingResponse>> GetById(int id, CancellationToken ct) => Ok(await _bindingService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateBindingRequest request, CancellationToken ct)
    {
        var id = await _bindingService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BindingResponse>> Update(int id, [FromBody] UpdateBindingRequest request, CancellationToken ct)
        => Ok(await _bindingService.UpdateAsync(id, request, ct));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _bindingService.DeleteAsync(id, ct);
        return NoContent();
    }
}
