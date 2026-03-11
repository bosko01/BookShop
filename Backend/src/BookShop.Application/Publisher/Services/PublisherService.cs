using BookShop.Application.Common.Exceptions;
using BookShop.Application.Interfaces.Persistence;
using BookShop.Application.Interfaces.Persistence.Common;
using BookShop.Application.Publisher.Contracts;
using BookShop.Application.Publisher.Contracts.Request;
using BookShop.Application.Publisher.Contracts.Response;

namespace BookShop.Application.Publisher.Services;

public sealed class PublisherService : IPublisherService
{
    private readonly IPublisherRepository _publisherRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PublisherService(IPublisherRepository publisherRepository, IUnitOfWork unitOfWork)
    {
        _publisherRepository = publisherRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<PublisherListResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        => (await _publisherRepository.GetAllAsync(cancellationToken))
            .Select(x => new PublisherListResponse(x.Id, x.Name, x.Address, x.City, x.Country, x.PhoneNumber))
            .ToList();

    public async Task<PublisherResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var publisher = await _publisherRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Publisher", id);
        return Map(publisher);
    }

    public async Task<int> CreateAsync(CreatePublisherRequest request, CancellationToken cancellationToken = default)
    {
        var publisher = Domain.Entities.Publisher.Create(request.Name, request.Address, request.City, request.Country, request.PhoneNumber);
        await _publisherRepository.AddAsync(publisher, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        return publisher.Id;
    }

    public async Task<PublisherResponse> UpdateAsync(int id, UpdatePublisherRequest request, CancellationToken cancellationToken = default)
    {
        var publisher = await _publisherRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Publisher", id);

        publisher.Rename(request.Name);
        publisher.UpdateContactInfo(request.Address, request.City, request.Country, request.PhoneNumber);
        await _unitOfWork.SaveAsync(cancellationToken);
        return Map(publisher);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var publisher = await _publisherRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Publisher", id);
        _publisherRepository.Remove(publisher);
        await _unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task<PublisherResponse> UpdateContactAsync(int id, UpdatePublisherContactRequest request, CancellationToken cancellationToken = default)
    {
        var publisher = await _publisherRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Publisher", id);
        publisher.UpdateContactInfo(request.Address, request.City, request.Country, request.PhoneNumber);
        await _unitOfWork.SaveAsync(cancellationToken);
        return Map(publisher);
    }

    private static PublisherResponse Map(Domain.Entities.Publisher p) => new(p.Id, p.Name, p.Address, p.City, p.Country, p.PhoneNumber);
}
