using BookShop.Application.Interfaces.Persistence.Common;
using System;
using System.Collections.Generic;
using System.Text;
using PublisherEntity = BookShop.Domain.Entities.Publisher;

namespace BookShop.Application.Interfaces.Persistence
{
    public interface IPublisherRepository : IRepository<PublisherEntity, int>
    {
    }
}
