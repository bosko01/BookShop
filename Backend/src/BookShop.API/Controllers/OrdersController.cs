using BookShop.Application.Order.Contracts;
using BookShop.Application.Order.Contracts.Request;
using BookShop.Application.Order.Contracts.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    public OrdersController(IOrderService orderService) => _orderService = orderService;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderResponse>>> GetAll(CancellationToken ct) => Ok(await _orderService.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderResponse>> GetById(int id, CancellationToken ct) => Ok(await _orderService.GetByIdAsync(id, ct));

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<int>> Create([FromBody] CreateOrderRequest request, CancellationToken ct)
    {
        var id = await _orderService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPatch("{id:int}/paid")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> MarkAsPaid(int id, CancellationToken ct) { await _orderService.MarkAsPaidAsync(id, ct); return NoContent(); }

    [HttpPatch("{id:int}/shipped")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> MarkAsShipped(int id, CancellationToken ct) { await _orderService.MarkAsShippedAsync(id, ct); return NoContent(); }

    [HttpPatch("{id:int}/delivered")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> MarkAsDelivered(int id, CancellationToken ct) { await _orderService.MarkAsDeliveredAsync(id, ct); return NoContent(); }

    [HttpPatch("{id:int}/cancel")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Cancel(int id, CancellationToken ct) { await _orderService.CancelAsync(id, ct); return NoContent(); }

    [HttpPatch("{id:int}/item-quantity")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeItemQuantity(int id, [FromBody] ChangeOrderItemQuantityRequest request, CancellationToken ct)
    {
        await _orderService.ChangeItemQuantityAsync(id, request, ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _orderService.DeleteAsync(id, ct);
        return NoContent();
    }
}
