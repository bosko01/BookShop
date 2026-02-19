namespace BookShop.Application.Book.Contracts.Request;

public sealed record UpdateBookRequest(
    string Title,
    decimal Price,
    int QuantityInStock,
    int PageCount,
    int PublisherId,
    int AuthorId,
    int GenreId,
    int BindingId,
    string? Description,
    string? ImageUrl
);
