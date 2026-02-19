using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BookShop.API.Security;
using BookShop.Application.Interfaces.Persistence;
using BookShop.Application.User.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BookShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRefreshTokenStore _refreshTokenStore;
    private readonly IConfiguration _configuration;

    public AuthController(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IRefreshTokenStore refreshTokenStore,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _refreshTokenStore = refreshTokenStore;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            return Unauthorized("Invalid email or password.");

        var token = GenerateAccessToken(user.Id, user.Email, user.Role.ToString());
        var refreshToken = GenerateRefreshToken();
        _refreshTokenStore.Store(refreshToken, user.Id, DateTime.UtcNow.AddDays(7));

        return Ok(new AuthResponse(token, refreshToken));
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponse>> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        if (!_refreshTokenStore.TryGet(request.RefreshToken, out var userId))
            return Unauthorized("Invalid refresh token.");

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return Unauthorized("Invalid refresh token.");

        _refreshTokenStore.Remove(request.RefreshToken);

        var accessToken = GenerateAccessToken(user.Id, user.Email, user.Role.ToString());
        var newRefreshToken = GenerateRefreshToken();
        _refreshTokenStore.Store(newRefreshToken, user.Id, DateTime.UtcNow.AddDays(7));

        return Ok(new AuthResponse(accessToken, newRefreshToken));
    }

    private string GenerateAccessToken(int userId, string email, string role)
    {
        var jwt = _configuration.GetSection("Jwt");
        var issuer = jwt["Issuer"]!;
        var audience = jwt["Audience"]!;
        var key = jwt["Key"]!;

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role)
        };

        var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }

    public sealed record LoginRequest(string Email, string Password);
    public sealed record RefreshTokenRequest(string RefreshToken);
    public sealed record AuthResponse(string AccessToken, string RefreshToken);
}
