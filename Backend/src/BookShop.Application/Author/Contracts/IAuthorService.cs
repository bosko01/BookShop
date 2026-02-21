using BookShop.Application.Author.Contracts;
using BookShop.Application.Author.Contracts.Request;
using BookShop.Application.Author.Contracts.Response;

namespace BookShop.Application.Author.Services;

public interface IAuthorService
{
    Task<AuthorResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AuthorListResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<int> CreateAsync(CreateAuthorRequest request, CancellationToken cancellationToken = default);
    Task<AuthorResponse> UpdateAsync(int id, UpdateAuthorRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
