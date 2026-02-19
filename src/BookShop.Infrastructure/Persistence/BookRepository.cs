using BookShop.Application.Interfaces.Persistence;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Persistence.Common;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Persistence;

public sealed class BookRepository : IBookRepository
{
    private readonly BookShopDbContext _dbContext;

    public BookRepository(BookShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Books
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Books
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Books
            .AnyAsync(b => b.Id == id, cancellationToken);
    }

    public async Task AddAsync(Book book, CancellationToken cancellationToken = default)
    {
        await _dbContext.Books.AddAsync(book, cancellationToken);
    }

    public void Update(Book entity)
    {
        _dbContext.Books.Update(entity);
    }

    public void Remove(Book book)
    {
        _dbContext.Books.Remove(book);
    }

    public async Task<bool> ExistsByTitleAndAuthorIdAsync(
        string title,
        int authorId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Books
            .AnyAsync(
                b => b.Title == title && b.AuthorId == authorId,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Book>> GetBooksByAuthorIdAsync(
        int authorId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Books
            .AsNoTracking()
            .Where(b => b.AuthorId == authorId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Book>> GetBooksByGenreAsync(
        string genre,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Books
            .AsNoTracking()
            .Where(b => b.Genre.ToString() == genre)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Book>> GetBooksByPublisherAsync(
        string publisher,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Books
            .AsNoTracking()
            .Where(b => b.Publisher.ToString() == publisher)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Book>> GetBooksWithLowStockAsync(
        int stockThreshold,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Books
            .AsNoTracking()
            .Where(b => b.QuantityInStock <= stockThreshold)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Book>> SearchBooksByTitleAsync(
        string titleKeyword,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(titleKeyword))
        {
            return Array.Empty<Book>();
        }

        var keyword = titleKeyword.Trim();

        return await _dbContext.Books
            .AsNoTracking()
            .Where(b => b.Title.Contains(keyword))
            .ToListAsync(cancellationToken);
    }
    public async Task<Book?> GetByIdIncludingDeletedAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Books
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

}
