using BookShop.Application.Review.Contracts.Request;
using BookShop.Application.Review.Contracts.Response;

namespace BookShop.Application.Review.Contracts;

public interface IReviewService
{
    Task<IReadOnlyList<ReviewResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ReviewResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ReviewResponse>> GetByBookIdAsync(int bookId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ReviewResponse>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(CreateReviewRequest request, CancellationToken cancellationToken = default);
    Task<ReviewResponse> UpdateAsync(int id, UpdateReviewRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
