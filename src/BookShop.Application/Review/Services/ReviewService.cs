using BookShop.Application.Common.Exceptions;
using BookShop.Application.Interfaces.Persistence;
using BookShop.Application.Interfaces.Persistence.Common;
using BookShop.Application.Review.Contracts;
using BookShop.Application.Review.Contracts.Request;
using BookShop.Application.Review.Contracts.Response;

namespace BookShop.Application.Review.Services;

public sealed class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReviewService(IReviewRepository reviewRepository, IBookRepository bookRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _bookRepository = bookRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<ReviewResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        => (await _reviewRepository.GetAllAsync(cancellationToken)).Select(Map).ToList();

    public async Task<ReviewResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var review = await _reviewRepository.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("Review", id);
        return Map(review);
    }

    public async Task<IReadOnlyList<ReviewResponse>> GetByBookIdAsync(int bookId, CancellationToken cancellationToken = default)
        => (await _reviewRepository.GetAllAsync(cancellationToken)).Where(x => x.BookId == bookId).Select(Map).ToList();

    public async Task<IReadOnlyList<ReviewResponse>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        => (await _reviewRepository.GetAllAsync(cancellationToken)).Where(x => x.UserId == userId).Select(Map).ToList();

    public async Task<int> CreateAsync(CreateReviewRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _bookRepository.ExistsAsync(request.BookId, cancellationToken))
            throw new NotFoundException("Book", request.BookId);
        if (!await _userRepository.ExistsAsync(request.UserId, cancellationToken))
            throw new NotFoundException("User", request.UserId);

        var review = Domain.Entities.Review.Create(request.BookId, request.UserId, request.Rating, request.Comment);
        await _reviewRepository.AddAsync(review, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        return review.Id;
    }

    public async Task<ReviewResponse> UpdateAsync(int id, UpdateReviewRequest request, CancellationToken cancellationToken = default)
    {
        var review = await _reviewRepository.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("Review", id);
        review.Update(request.Rating, request.Comment);
        await _unitOfWork.SaveAsync(cancellationToken);
        return Map(review);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var review = await _reviewRepository.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("Review", id);
        _reviewRepository.Remove(review);
        await _unitOfWork.SaveAsync(cancellationToken);
    }

    private static ReviewResponse Map(Domain.Entities.Review review) => new(review.Id, review.BookId, review.UserId, review.Rating, review.Comment);
}
