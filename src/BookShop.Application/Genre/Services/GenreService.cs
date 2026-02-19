using BookShop.Application.Common.Exceptions;
using BookShop.Application.Genre.Contracts;
using BookShop.Application.Genre.Contracts.Request;
using BookShop.Application.Genre.Contracts.Response;
using BookShop.Application.Interfaces.Persistence;
using BookShop.Application.Interfaces.Persistence.Common;

namespace BookShop.Application.Genre.Services;

public sealed class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GenreService(IGenreRepository genreRepository, IUnitOfWork unitOfWork)
    {
        _genreRepository = genreRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<GenreResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        => (await _genreRepository.GetAllAsync(cancellationToken)).Select(x => new GenreResponse(x.Id, x.Name)).ToList();

    public async Task<GenreResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var genre = await _genreRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Genre", id);
        return new GenreResponse(genre.Id, genre.Name);
    }

    public async Task<int> CreateAsync(CreateGenreRequest request, CancellationToken cancellationToken = default)
    {
        var genre = Domain.Entities.Genre.Create(request.Name);
        await _genreRepository.AddAsync(genre, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        return genre.Id;
    }

    public async Task<GenreResponse> UpdateAsync(int id, UpdateGenreRequest request, CancellationToken cancellationToken = default)
    {
        var genre = await _genreRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Genre", id);
        genre.Rename(request.Name);
        await _unitOfWork.SaveAsync(cancellationToken);
        return new GenreResponse(genre.Id, genre.Name);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var genre = await _genreRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Genre", id);
        _genreRepository.Remove(genre);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}
