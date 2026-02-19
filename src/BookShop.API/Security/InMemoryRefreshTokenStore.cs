using System.Collections.Concurrent;

namespace BookShop.API.Security;

public sealed class InMemoryRefreshTokenStore : IRefreshTokenStore
{
    private readonly ConcurrentDictionary<string, RefreshTokenEntry> _tokens = new();

    public void Store(string refreshToken, int userId, DateTime expiresAtUtc)
        => _tokens[refreshToken] = new RefreshTokenEntry(userId, expiresAtUtc);

    public bool TryGet(string refreshToken, out int userId)
    {
        userId = default;

        if (!_tokens.TryGetValue(refreshToken, out var entry))
            return false;

        if (entry.ExpiresAtUtc <= DateTime.UtcNow)
        {
            _tokens.TryRemove(refreshToken, out _);
            return false;
        }

        userId = entry.UserId;
        return true;
    }

    public void Remove(string refreshToken) => _tokens.TryRemove(refreshToken, out _);

    private sealed record RefreshTokenEntry(int UserId, DateTime ExpiresAtUtc);
}
