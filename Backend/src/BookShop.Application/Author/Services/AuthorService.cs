using BookShop.Application.Author.Contracts;
using BookShop.Application.Author.Contracts.Request;
using BookShop.Application.Author.Contracts.Response;
using BookShop.Application.Common.Exceptions;
using BookShop.Application.Interfaces.Persistence;
using BookShop.Application.Interfaces.Persistence.Common;
using BookShop.Domain.Entities;

namespace BookShop.Application.Author.Services;

public sealed class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AuthorService(IAuthorRepository authorRepository, IUnitOfWork unitOfWork)
    {
        _authorRepository = authorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthorResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var author = await _authorRepository.GetByIdAsync(id, cancellationToken);
        if (author is null)
            throw new NotFoundException("Author", id);

        return new AuthorResponse(author.FirstName, author.LastName, author.Biography);
    }

    public async Task<IReadOnlyList<AuthorListResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var authors = await _authorRepository.GetAllAsync(cancellationToken);

        if (!authors.Any())
            throw new NotFoundException("There are no Authors to display!");

        return authors
            .Select(a => new AuthorListResponse(a.FirstName, a.LastName, a.GetFullName()))
            .ToList();
    }

    public async Task<int> CreateAsync(CreateAuthorRequest request, CancellationToken cancellationToken = default)
    {
        var exists = await _authorRepository.ExistsByFullNameAsync(request.FirstName, request.LastName, cancellationToken);
        if (exists) throw new ConflictException("Author already exists.");

        var author = BookShop.Domain.Entities.Author.Create(request.FirstName, request.LastName, request.Biography);

        await _authorRepository.AddAsync(author, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return author.Id;
    }

    public async Task<AuthorResponse> UpdateAsync(int id, UpdateAuthorRequest request, CancellationToken cancellationToken = default)
    {
        var author = await _authorRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Author", id);

        author.Rename(request.FirstName, request.LastName);
        author.UpdateBiography(request.Biography);

        await _unitOfWork.SaveAsync(cancellationToken);
        return new AuthorResponse(request.FirstName, request.LastName, request.Biography);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var author = await _authorRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Author", id);

        _authorRepository.Remove(author);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}
