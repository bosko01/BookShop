using BookShop.Application.PaymentMethod.Contracts;
using BookShop.Application.PaymentMethod.Contracts.Request;
using BookShop.Application.PaymentMethod.Contracts.Response;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class PaymentMethodsController : ControllerBase
{
    private readonly IPaymentMethodService _paymentMethodService;
    public PaymentMethodsController(IPaymentMethodService paymentMethodService) => _paymentMethodService = paymentMethodService;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PaymentMethodResponse>>> GetAll(CancellationToken ct) => Ok(await _paymentMethodService.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PaymentMethodResponse>> GetById(int id, CancellationToken ct) => Ok(await _paymentMethodService.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreatePaymentMethodRequest request, CancellationToken ct)
    {
        var id = await _paymentMethodService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PaymentMethodResponse>> Update(int id, [FromBody] UpdatePaymentMethodRequest request, CancellationToken ct)
        => Ok(await _paymentMethodService.UpdateAsync(id, request, ct));

    [HttpPatch("{id:int}/activate")]
    public async Task<IActionResult> Activate(int id, CancellationToken ct)
    {
        await _paymentMethodService.ActivateAsync(id, ct);
        return NoContent();
    }

    [HttpPatch("{id:int}/deactivate")]
    public async Task<IActionResult> Deactivate(int id, CancellationToken ct)
    {
        await _paymentMethodService.DeactivateAsync(id, ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _paymentMethodService.DeleteAsync(id, ct);
        return NoContent();
    }
}
