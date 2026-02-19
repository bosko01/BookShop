using BookShop.Application.User.Contracts;
using BookShop.Application.User.Contracts.Request;
using BookShop.Application.User.Contracts.Response;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserResponse>>> GetAll(CancellationToken ct) => Ok(await _userService.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResponse>> GetById(int id, CancellationToken ct) => Ok(await _userService.GetByIdAsync(id, ct));

    [HttpGet("by-email")]
    public async Task<ActionResult<UserResponse>> GetByEmail([FromQuery] string email, CancellationToken ct) => Ok(await _userService.GetByEmailAsync(email, ct));

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateUserRequest request, CancellationToken ct)
    {
        var id = await _userService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserResponse>> Update(int id, [FromBody] UpdateUserRequest request, CancellationToken ct)
        => Ok(await _userService.UpdateAsync(id, request, ct));

    [HttpPatch("{id:int}/password")]
    public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangeUserPasswordRequest request, CancellationToken ct)
    {
        await _userService.ChangePasswordAsync(id, request, ct);
        return NoContent();
    }

    [HttpPatch("{id:int}/role")]
    public async Task<IActionResult> SetRole(int id, [FromBody] SetUserRoleRequest request, CancellationToken ct)
    {
        await _userService.SetRoleAsync(id, request, ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _userService.DeleteAsync(id, ct);
        return NoContent();
    }
}
