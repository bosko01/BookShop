using BookShop.Application.Invoice.Contracts;
using BookShop.Application.Invoice.Contracts.Request;
using BookShop.Application.Invoice.Contracts.Response;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class InvoicesController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;
    public InvoicesController(IInvoiceService invoiceService) => _invoiceService = invoiceService;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<InvoiceResponse>>> GetAll(CancellationToken ct) => Ok(await _invoiceService.GetAllAsync(ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<InvoiceResponse>> GetById(Guid id, CancellationToken ct) => Ok(await _invoiceService.GetByIdAsync(id, ct));


    [HttpGet("by-order/{orderId:int}")]
    public async Task<ActionResult<InvoiceResponse>> GetByOrderId(int orderId, CancellationToken ct) => Ok(await _invoiceService.GetByOrderIdAsync(orderId, ct));

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateInvoiceRequest request, CancellationToken ct)
    {
        var id = await _invoiceService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPatch("{id:guid}/paid")]
    public async Task<IActionResult> MarkAsPaid(Guid id, [FromBody] MarkInvoicePaidRequest request, CancellationToken ct)
    {
        await _invoiceService.MarkAsPaidAsync(id, request, ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _invoiceService.DeleteAsync(id, ct);
        return NoContent();
    }
}
