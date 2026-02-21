using BookShop.Application.Interfaces.Persistence.Common;
using System;
using System.Collections.Generic;
using System.Text;
using ReviewEntity = BookShop.Domain.Entities.Review;

namespace BookShop.Application.Interfaces.Persistence
{
    public interface IReviewRepository : IRepository<ReviewEntity, int>
    {
    }
}
