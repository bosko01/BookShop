using BookShop.Application.Interfaces.Persistence.Common;
using BookShop.Domain.Entities;

namespace BookShop.Application.Interfaces.Persistence;

public interface IUserRepository : IRepository<User, int>
{
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

}
