using BookShop.Application.Interfaces.Persistence;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Persistence.Common;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Persistence;

public sealed class AuthorRepository : IAuthorRepository
{
    private readonly BookShopDbContext _dbContext;

    public AuthorRepository(BookShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Author?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Authors
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Author>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Authors
            .AsNoTracking()
            .OrderBy(a => a.LastName)
            .ThenBy(a => a.FirstName)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByFullNameAsync(
        string firstName,
        string lastName,
        CancellationToken cancellationToken = default)
    {
        var normalizedFirst = firstName.Trim();
        var normalizedLast = lastName.Trim();

        return await _dbContext.Authors.AnyAsync(
            a => a.FirstName == normalizedFirst &&
                 a.LastName == normalizedLast,
            cancellationToken);
    }

    public async Task AddAsync(
        Author author,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Authors.AddAsync(author, cancellationToken);
    }

    public void Remove(Author author)
    {
        _dbContext.Authors.Remove(author);
    }

    public async Task<bool> ExistsAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Authors
            .AnyAsync(a => a.Id == id, cancellationToken);
    }

    public void Update(Author entity)
    {
        _dbContext.Authors.Update(entity);
    }
}
