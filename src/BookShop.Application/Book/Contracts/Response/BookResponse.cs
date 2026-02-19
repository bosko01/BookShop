using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Application.Book.Contracts.Response
{
    public sealed record BookResponse(
    int Id,
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
}
