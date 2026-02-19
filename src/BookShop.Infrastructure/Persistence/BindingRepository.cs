using BookShop.Application.Common.Exceptions;
using BookShop.Application.Interfaces.Persistence;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Persistence.Common;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Persistence;

public sealed class BindingRepository : IBindingRepository
{
    private readonly BookShopDbContext _dbContext;

    public BindingRepository(BookShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Binding?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var book =  await _dbContext.Bindings
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        if ( book is null )
        {
            throw new NotFoundException("Book", id);
        }
        return book;
    }

    public async Task<IReadOnlyList<Binding>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Bindings
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Bindings
            .AnyAsync(b => b.Id == id, cancellationToken);
    }

    public async Task AddAsync(
        Binding entity,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Bindings
            .AddAsync(entity, cancellationToken);
    }

    public void Update(Binding entity)
    {
        _dbContext.Bindings.Update(entity);
    }

    public void Remove(Binding entity)
    {
        _dbContext.Bindings.Remove(entity);
    }
}