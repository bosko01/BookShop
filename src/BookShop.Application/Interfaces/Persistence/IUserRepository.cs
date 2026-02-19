using BookShop.Application.Interfaces.Persistence.Common;
using BookShop.Domain.Entities;
using UserEntity = BookShop.Domain.Entities.User;
namespace BookShop.Application.Interfaces.Persistence;

public interface IUserRepository : IRepository<UserEntity, int>
{
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

}
