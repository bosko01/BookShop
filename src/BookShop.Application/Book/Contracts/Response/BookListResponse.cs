using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Application.Book.Contracts.Response
{
    public sealed record BookListResponse(
    int Id,
    string Title,
    decimal Price,
    int QuantityInStock
);
}
