using BookShop.Application.Interfaces.Persistence;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Persistence.Common;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Persistence;

public sealed class OrderRepository : IOrderRepository
{
    private readonly BookShopDbContext _dbContext;

    public OrderRepository(BookShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<Order?> GetByIdWithItemsAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders
            .Include(o => o.Items)
            .Include(o => o.Invoice)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Order>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders
            .AnyAsync(o => o.Id == id, cancellationToken);
    }

    public async Task AddAsync(
        Order entity,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Orders
            .AddAsync(entity, cancellationToken);
    }

    public void Update(Order entity)
    {
        _dbContext.Orders.Update(entity);
    }

    public void Remove(Order entity)
    {
        _dbContext.Orders.Remove(entity);
    }
}
