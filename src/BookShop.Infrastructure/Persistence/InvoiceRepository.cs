using BookShop.Application.Interfaces.Persistence;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Persistence.Common;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Persistence;

public sealed class InvoiceRepository : IInvoiceRepository
{
    private readonly BookShopDbContext _dbContext;

    public InvoiceRepository(BookShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Invoice?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Invoices
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Invoice>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Invoices
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Invoices
            .AnyAsync(i => i.Id == id, cancellationToken);
    }

    public async Task AddAsync(
        Invoice entity,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Invoices
            .AddAsync(entity, cancellationToken);
    }

    public void Update(Invoice entity)
    {
        _dbContext.Invoices.Update(entity);
    }

    public void Remove(Invoice entity)
    {
        _dbContext.Invoices.Remove(entity);
    }
}
