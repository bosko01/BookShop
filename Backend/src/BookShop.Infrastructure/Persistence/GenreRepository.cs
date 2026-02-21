using BookShop.Application.Interfaces.Persistence;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Persistence.Common;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Persistence;

public sealed class GenreRepository : IGenreRepository
{
    private readonly BookShopDbContext _dbContext;

    public GenreRepository(BookShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Genre?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Genres
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Genre>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Genres
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Genres
            .AnyAsync(g => g.Id == id, cancellationToken);
    }

    public async Task AddAsync(
        Genre entity,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Genres
            .AddAsync(entity, cancellationToken);
    }

    public void Update(Genre entity)
    {
        _dbContext.Genres.Update(entity);
    }

    public void Remove(Genre entity)
    {
        _dbContext.Genres.Remove(entity);
    }
}