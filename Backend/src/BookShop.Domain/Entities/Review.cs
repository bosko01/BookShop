namespace BookShop.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookShop.Domain.Common;

public class Review : SoftDeleteBaseEntity
{
    private Review() { } // EF

    private Review(int bookId, int userId, int rating, string? comment)
    {
        BookId = bookId;
        UserId = userId;
        Rating = rating;
        Comment = comment;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public int BookId { get; private set; }

    public int UserId { get; private set; }

    [Range(1, 5)]
    public int Rating { get; private set; }

    [MaxLength(2000)]
    public string? Comment { get; private set; }

    public Book Book { get; private set; } = default!;
    public User User { get; private set; } = default!;

    public static Review Create(int bookId, int userId, int rating, string? comment = null)
    {
        if (bookId <= 0)
            throw new ArgumentOutOfRangeException(nameof(bookId));

        if (userId <= 0)
            throw new ArgumentOutOfRangeException(nameof(userId));

        if (rating < 1 || rating > 5)
            throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");

        if (comment is not null && comment.Length > 2000)
            throw new ArgumentException("Comment is too long.", nameof(comment));

        return new Review(bookId, userId, rating, comment);
    }

    public void Update(int rating, string? comment)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentOutOfRangeException(nameof(rating));

        if (comment is not null && comment.Length > 2000)
            throw new ArgumentException("Comment is too long.", nameof(comment));

        Rating = rating;
        Comment = comment;
    }
}
