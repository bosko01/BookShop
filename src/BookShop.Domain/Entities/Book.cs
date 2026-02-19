using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookShop.Domain.Common;

namespace BookShop.Domain.Entities;

public class Book : SoftDeleteBaseEntity
{
    private Book() { }

    private Book(
        string title,
        decimal price,
        int quantityInStock,
        int pageCount,
        int publisherId,
        int authorId,
        int genreId,
        int bindingId,
        string? description,
        string? imageUrl)
    {
        Title = title;
        Price = price;
        QuantityInStock = quantityInStock;
        PageCount = pageCount;
        PublisherId = publisherId;
        AuthorId = authorId;
        GenreId = genreId;
        BindingId = bindingId;
        Description = description;
        ImageUrl = imageUrl;
    }

    [Required, MaxLength(250)]
    public string Title { get; private set; } = default!;

    [MaxLength(4000)]
    public string? Description { get; private set; }

    [MaxLength(500)]
    public string? ImageUrl { get; private set; }

    public int PageCount { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; private set; }

    public int QuantityInStock { get; private set; }

    public int PublisherId { get; private set; }
    public int AuthorId { get; private set; }
    public int GenreId { get; private set; }
    public int BindingId { get; private set; }

    // Navigation (EF Core)
    public Publisher Publisher { get; private set; } = default!;
    public Author Author { get; private set; } = default!;
    public Genre Genre { get; private set; } = default!;
    public Binding Binding { get; private set; } = default!;
    public ICollection<Review> Reviews { get; private set; } = new List<Review>();


    public static Book Create(
        string title,
        decimal price,
        int quantityInStock,
        int pageCount,
        int publisherId,
        int authorId,
        int genreId,
        int bindingId,
        string? description = null,
        string? imageUrl = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));

        if (price <= 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Price must be greater than zero.");

        if (quantityInStock < 0)
            throw new ArgumentOutOfRangeException(nameof(quantityInStock), "Stock cannot be negative.");

        if (pageCount < 0)
            throw new ArgumentOutOfRangeException(nameof(pageCount), "Page count cannot be negative.");

        if (publisherId <= 0)
            throw new ArgumentOutOfRangeException(nameof(publisherId));

        if (authorId <= 0)
            throw new ArgumentOutOfRangeException(nameof(authorId));

        if (genreId <= 0)
            throw new ArgumentOutOfRangeException(nameof(genreId));

        if (bindingId <= 0)
            throw new ArgumentOutOfRangeException(nameof(bindingId));

        return new Book(
            title.Trim(),
            price,
            quantityInStock,
            pageCount,
            publisherId,
            authorId,
            genreId,
            bindingId,
            description,
            imageUrl);
    }

    public void ChangeTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            throw new ArgumentException("Title cannot be empty.", nameof(newTitle));

        Title = newTitle.Trim();
    }

    public void ChangePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentOutOfRangeException(nameof(newPrice), "Price must be greater than zero.");

        Price = newPrice;
    }

    public void ChangePageCount(int pageCount)
    {
        if (pageCount < 0)
            throw new ArgumentOutOfRangeException(nameof(pageCount), "Page count cannot be negative.");

        PageCount = pageCount;
    }

    public void SetQuantityInStock(int newQuantity)
    {
        if (newQuantity < 0)
            throw new ArgumentOutOfRangeException(nameof(newQuantity), "Stock cannot be negative.");

        QuantityInStock = newQuantity;
    }

    public void DecrementStock(int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");

        if (amount > QuantityInStock)
            throw new InvalidOperationException("Not enough stock to decrement.");

        QuantityInStock -= amount;
    }

    public void ChangePublisher(int publisherId)
    {
        if (publisherId <= 0)
            throw new ArgumentOutOfRangeException(nameof(publisherId));

        PublisherId = publisherId;
    }

    public void ChangeAuthor(int authorId)
    {
        if (authorId <= 0)
            throw new ArgumentOutOfRangeException(nameof(authorId));

        AuthorId = authorId;
    }

    public void ChangeGenre(int genreId)
    {
        if (genreId <= 0)
            throw new ArgumentOutOfRangeException(nameof(genreId));

        GenreId = genreId;
    }

    public void ChangeBinding(int bindingId)
    {
        if (bindingId <= 0)
            throw new ArgumentOutOfRangeException(nameof(bindingId));

        BindingId = bindingId;
    }

    public void UpdateDescription(string? newDescription)
    {
        Description = newDescription;
    }

    public void UpdateImageUrl(string? newImageUrl)
    {
        ImageUrl = newImageUrl;
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
        DeletedAtUtc = DateTime.UtcNow;
    }

    public void Restore()
    {
        if (!IsDeleted)
            throw new InvalidOperationException("Book is not deleted.");

        IsDeleted = false;
        DeletedAtUtc = null;
    }

}
