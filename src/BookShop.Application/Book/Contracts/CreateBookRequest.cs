using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Application.Book.Contracts
{
    public sealed record CreateBookRequest(
        string Title,
        decimal Price,
        int PublisherId
    );
}