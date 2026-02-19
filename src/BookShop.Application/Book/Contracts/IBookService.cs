using BookShop.Application.Book.Contracts.Request;
using BookShop.Application.Book.Contracts.Response;

namespace BookShop.Application.Book.Contracts;

public interface IBookService
{
    // Queries
    Task<BookResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BookListResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BookListResponse>> GetByAuthorIdAsync(int authorId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BookListResponse>> GetByGenreAsync(string genre, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BookListResponse>> GetByPublisherAsync(string publisher, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BookListResponse>> GetLowStockAsync(int stockThreshold, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BookListResponse>> SearchByTitleAsync(string titleKeyword, CancellationToken cancellationToken = default);

    Task<int> CreateAsync(CreateBookRequest request, CancellationToken cancellationToken = default);
    Task<BookResponse> UpdateAsync(int id, UpdateBookRequest request, CancellationToken cancellationToken = default);

    Task DecrementStockAsync(int bookId, int amount, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task RestoreAsync(int id, CancellationToken cancellationToken = default);


}
