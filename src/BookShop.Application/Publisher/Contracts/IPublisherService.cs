using BookShop.Application.Publisher.Contracts.Request;
using BookShop.Application.Publisher.Contracts.Response;

namespace BookShop.Application.Publisher.Contracts;

public interface IPublisherService
{
    Task<IReadOnlyList<PublisherListResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PublisherResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(CreatePublisherRequest request, CancellationToken cancellationToken = default);
    Task<PublisherResponse> UpdateAsync(int id, UpdatePublisherRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<PublisherResponse> UpdateContactAsync(int id, UpdatePublisherContactRequest request, CancellationToken cancellationToken = default);
}
