using BookShop.Application.Interfaces.Persistence;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Persistence.Common;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Persistence;

public sealed class PublisherRepository : IPublisherRepository
{
    private readonly BookShopDbContext _dbContext;

    public PublisherRepository(BookShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Publisher?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Publishers
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Publisher>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Publishers
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Publishers
            .AnyAsync(p => p.Id == id, cancellationToken);
    }

    public async Task AddAsync(
        Publisher entity,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Publishers
            .AddAsync(entity, cancellationToken);
    }

    public void Update(Publisher entity)
    {
        _dbContext.Publishers.Update(entity);
    }

    public void Remove(Publisher entity)
    {
        _dbContext.Publishers.Remove(entity);
    }
}
