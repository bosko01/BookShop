using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Application.Book.Contracts.Request
{
    public sealed record CreateBookRequest(
        string Title,
    string? Description,
    string? ImageUrl,
    int PageCount,
    decimal Price,
    int QuantityInStock,
    int AuthorId,
    int PublisherId,
    int GenreId,
    int BindingId
    );

}