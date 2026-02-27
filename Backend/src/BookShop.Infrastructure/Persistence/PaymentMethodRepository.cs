using BookShop.Application.Interfaces.Persistence;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Persistence.Common;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Persistence;

public sealed class PaymentMethodRepository : IPaymentMethodRepository
{
    private readonly BookShopDbContext _dbContext;

    public PaymentMethodRepository(BookShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaymentMethod?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.PaymentMethods
            .FirstOrDefaultAsync(pm => pm.Id == id, cancellationToken);
    }


    public async Task<PaymentMethod?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.PaymentMethods
            .FirstOrDefaultAsync(pm => pm.Name == name, cancellationToken);
    }
    public async Task<IReadOnlyList<PaymentMethod>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.PaymentMethods
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.PaymentMethods
            .AnyAsync(pm => pm.Id == id, cancellationToken);
    }

    public async Task AddAsync(
        PaymentMethod entity,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.PaymentMethods
            .AddAsync(entity, cancellationToken);
    }

    public void Update(PaymentMethod entity)
    {
        _dbContext.PaymentMethods.Update(entity);
    }

    public void Remove(PaymentMethod entity)
    {
        _dbContext.PaymentMethods.Remove(entity);
    }
}
