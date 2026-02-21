using BookShop.Application.Genre.Contracts.Request;
using BookShop.Application.Genre.Contracts.Response;

namespace BookShop.Application.Genre.Contracts;

public interface IGenreService
{
    Task<IReadOnlyList<GenreResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<GenreResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(CreateGenreRequest request, CancellationToken cancellationToken = default);
    Task<GenreResponse> UpdateAsync(int id, UpdateGenreRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
