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
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Review>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Reviews
            .Include(r => r.User)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Review>> GetByBookIdAsync(
        int bookId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Reviews
            .Include(r => r.User)
            .AsNoTracking()
            .Where(r => r.BookId == bookId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Review>> GetByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Reviews
            .Include(r => r.User)
            .AsNoTracking()
            .Where(r => r.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Review?> GetByBookAndUserAsync(
        int bookId,
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Reviews
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.BookId == bookId && r.UserId == userId, cancellationToken);
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
