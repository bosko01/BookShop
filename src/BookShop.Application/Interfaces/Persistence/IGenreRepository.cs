using BookShop.Application.Interfaces.Persistence.Common;
using System;
using System.Collections.Generic;
using System.Text;
using GenreEntity = BookShop.Domain.Entities.Genre;
namespace BookShop.Application.Interfaces.Persistence
{
    public interface IGenreRepository : IRepository<GenreEntity, int>
    {
    }
}
