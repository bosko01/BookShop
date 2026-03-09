using BookShop.Application.Book.Contracts;
using BookShop.Application.Book.Contracts.Request;
using BookShop.Application.Book.Contracts.Response;
using BookShop.Application.Common.Exceptions;
using BookShop.Application.Interfaces.Persistence;
using BookShop.Application.Interfaces.Persistence.Common;
using FluentValidation;
using BookEntiy = BookShop.Domain.Entities.Book;

namespace BookShop.Application.Book.Services;

public sealed class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IPublisherRepository _publisherRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IBindingRepository _bindingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BookService(
        IBookRepository bookRepository,
        IAuthorRepository authorRepository,
        IPublisherRepository publisherRepository,
        IGenreRepository genreRepository,
        IBindingRepository bindingRepository,
        IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _publisherRepository = publisherRepository;
        _genreRepository = genreRepository;
        _bindingRepository = bindingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BookResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var book = await _bookRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Book", id);

        return MapToResponse(book);
    }

    public async Task<IReadOnlyList<BookListResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var books = await _bookRepository.GetAllAsync(cancellationToken);
        return books.Select(MapToListResponse).ToList();
    }

    public async Task<int> CreateAsync(CreateBookRequest request, CancellationToken cancellationToken = default)
    {
        await EnsureLookupsExistAsync(
            request.AuthorId,
            request.PublisherId,
            request.GenreId,
            request.BindingId,
            cancellationToken);

        var exists = await _bookRepository.ExistsByTitleAndAuthorIdAsync(
            request.Title,
            request.AuthorId,
            cancellationToken);

        if (exists)
            throw new ConflictException("Book already exists for this author.");

        var book = BookEntiy.Create(
            request.Title,
            request.Price,
            request.QuantityInStock,
            request.PageCount,
            request.PublisherId,
            request.AuthorId,
            request.GenreId,
            request.BindingId,
            request.Description,
            request.ImageUrl);

        await _bookRepository.AddAsync(book, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return book.Id;
    }

    public async Task<BookResponse> UpdateAsync(int id, UpdateBookRequest request, CancellationToken cancellationToken = default)
    {
        var book = await _bookRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Book", id);

        await EnsureLookupsExistAsync(
            request.AuthorId,
            request.PublisherId,
            request.GenreId,
            request.BindingId,
            cancellationToken);

        var wouldConflict = await _bookRepository.ExistsByTitleAndAuthorIdAsync(
            request.Title,
            request.AuthorId,
            cancellationToken);

        if (wouldConflict && (book.AuthorId != request.AuthorId || book.Title != request.Title))
            throw new ConflictException("Book already exists for this author.");

        if (book.Title != request.Title) book.ChangeTitle(request.Title);
        if (book.Price != request.Price) book.ChangePrice(request.Price);
        if (book.QuantityInStock != request.QuantityInStock) book.SetQuantityInStock(request.QuantityInStock);

        if (book.PageCount != request.PageCount) book.ChangePageCount(request.PageCount);
        if (book.PublisherId != request.PublisherId) book.ChangePublisher(request.PublisherId);
        if (book.AuthorId != request.AuthorId) book.ChangeAuthor(request.AuthorId);
        if (book.GenreId != request.GenreId) book.ChangeGenre(request.GenreId);
        if (book.BindingId != request.BindingId) book.ChangeBinding(request.BindingId);

        if (book.Description != request.Description) book.UpdateDescription(request.Description);
        if (book.ImageUrl != request.ImageUrl) book.UpdateImageUrl(request.ImageUrl);

        await _unitOfWork.SaveAsync(cancellationToken);

        return MapToResponse(book);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var book = await _bookRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Book", id);

        book.MarkAsDeleted();
        await _unitOfWork.SaveAsync(cancellationToken);
    }

    private async Task EnsureLookupsExistAsync(
        int authorId,
        int publisherId,
        int genreId,
        int bindingId,
        CancellationToken cancellationToken)
    {
        if (!await _authorRepository.ExistsAsync(authorId, cancellationToken))
            throw new NotFoundException("Author", authorId);

        if (!await _publisherRepository.ExistsAsync(publisherId, cancellationToken))
            throw new NotFoundException("Publisher", publisherId);

        if (!await _genreRepository.ExistsAsync(genreId, cancellationToken))
            throw new NotFoundException("Genre", genreId);

        if (!await _bindingRepository.ExistsAsync(bindingId, cancellationToken))
            throw new NotFoundException("Binding", bindingId);
    }

    private static BookResponse MapToResponse(BookEntiy book)
        => new(
            book.Id,
            book.Title,
            book.Price,
            book.QuantityInStock,
            book.PageCount,
            book.PublisherId,
            book.AuthorId,
            book.GenreId,
            book.BindingId,
            book.Description,
            book.ImageUrl);

    private static BookListResponse MapToListResponse(BookEntiy book)
        => new(
            book.Id,
            book.Title,
            book.Price,
            book.QuantityInStock,
            book.ImageUrl);

    public async Task<IReadOnlyList<BookListResponse>> GetByAuthorIdAsync(
    int authorId,
    CancellationToken cancellationToken = default)
    {
        var authorExists = await _authorRepository.ExistsAsync(authorId, cancellationToken);
        if (!authorExists)
            throw new NotFoundException("Author", authorId);

        var books = await _bookRepository.GetBooksByAuthorIdAsync(authorId, cancellationToken);

        return books
            .Select(MapToListResponse)
            .ToList();
    }


    public async Task<IReadOnlyList<BookListResponse>> GetByGenreAsync(
    string genre,
    CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(genre))
            return Array.Empty<BookListResponse>();

        var books = await _bookRepository.GetBooksByGenreAsync(genre.Trim(), cancellationToken);

        return books
            .Select(MapToListResponse)
            .ToList();
    }


    public async Task<IReadOnlyList<BookListResponse>> GetByPublisherAsync(
    string publisher,
    CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(publisher))
            return Array.Empty<BookListResponse>();

        var books = await _bookRepository.GetBooksByPublisherAsync(publisher.Trim(), cancellationToken);

        return books
            .Select(MapToListResponse)
            .ToList();
    }


    public async Task<IReadOnlyList<BookListResponse>> GetLowStockAsync(
    int stockThreshold,
    CancellationToken cancellationToken = default)
    {
        if (stockThreshold < 0)
            throw new ValidationException("Stock threshold must be non-negative.");

        var books = await _bookRepository.GetBooksWithLowStockAsync(stockThreshold, cancellationToken);

        return books
            .Select(MapToListResponse)
            .ToList();
    }


    public async Task<IReadOnlyList<BookListResponse>> SearchByTitleAsync(
    string titleKeyword,
    CancellationToken cancellationToken = default)
    {
        var books = await _bookRepository.SearchBooksByTitleAsync(titleKeyword, cancellationToken);

        return books
            .Select(MapToListResponse)
            .ToList();
    }


    public async Task DecrementStockAsync(
    int bookId,
    int amount,
    CancellationToken cancellationToken = default)
    {
        var book = await _bookRepository.GetByIdAsync(bookId, cancellationToken)
            ?? throw new NotFoundException("Book", bookId);

        book.DecrementStock(amount);

        await _unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task RestoreAsync(int id, CancellationToken cancellationToken = default)
    {
        var book = await _bookRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Book", id);

        book.Restore();

        await _unitOfWork.SaveAsync(cancellationToken);
    }

}
