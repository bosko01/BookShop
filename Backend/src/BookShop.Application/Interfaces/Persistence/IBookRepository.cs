using BookShop.Application.Interfaces.Persistence.Common;
using System;
using System.Collections.Generic;
using System.Text;
using BookEntity = BookShop.Domain.Entities.Book;
namespace BookShop.Application.Interfaces.Persistence
{
    public interface IBookRepository : IRepository<BookEntity, int>
    {
        
        Task<bool> ExistsByTitleAndAuthorIdAsync(
            string title,
            int authorId,
            CancellationToken cancellationToken = default);
        Task<IReadOnlyList<BookEntity>> GetBooksByAuthorIdAsync(
            int authorId,
            CancellationToken cancellationToken = default);
        Task<IReadOnlyList<BookEntity>> SearchBooksByTitleAsync(
            string titleKeyword,
            CancellationToken cancellationToken = default);
        Task<IReadOnlyList<BookEntity>> GetBooksByGenreAsync(
            string genre,
            CancellationToken cancellationToken = default);
        Task<IReadOnlyList<BookEntity>> GetBooksWithLowStockAsync(
            int stockThreshold,
            CancellationToken cancellationToken = default);
        Task<IReadOnlyList<BookEntity>> GetBooksByPublisherAsync(
            string publisher,
            CancellationToken cancellationToken = default);
        Task<BookEntity?> GetByIdIncludingDeletedAsync(int id, CancellationToken cancellationToken = default);

    }
}
