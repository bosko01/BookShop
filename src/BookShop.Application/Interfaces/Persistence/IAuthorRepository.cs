namespace BookShop.Application.Interfaces.Persistence;

using BookShop.Application.Interfaces.Persistence.Common;
using Author = BookShop.Domain.Entities.Author;

public interface IAuthorRepository : IRepository<Author, int>
{
   Task<bool> ExistsByFullNameAsync(string firstName, string lastName, CancellationToken cancellationToken);
}