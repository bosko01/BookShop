namespace BookShop.API.Security;

public interface IRefreshTokenStore
{
    void Store(string refreshToken, int userId, DateTime expiresAtUtc);
    bool TryGet(string refreshToken, out int userId);
    void Remove(string refreshToken);
}
