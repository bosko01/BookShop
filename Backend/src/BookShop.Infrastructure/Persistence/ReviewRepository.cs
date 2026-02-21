using BookShop.Application.Interfaces.Persistence;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Persistence.Common;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Persistence;

public sealed class ReviewRepository : IReviewRepository
{
    private readonly BookShopDbContext _dbContext;

    public ReviewRepository(BookShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Review?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Reviews
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Review>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Reviews
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Reviews
            .AnyAsync(r => r.Id == id, cancellationToken);
    }

    public async Task AddAsync(
        Review entity,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Reviews
            .AddAsync(entity, cancellationToken);
    }

    public void Update(Review entity)
    {
        _dbContext.Reviews.Update(entity);
    }

    public void Remove(Review entity)
    {
        _dbContext.Reviews.Remove(entity);
    }
}
