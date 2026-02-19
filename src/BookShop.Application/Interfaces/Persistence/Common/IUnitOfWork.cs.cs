using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Application.Interfaces.Persistence.Common
{
    public interface IUnitOfWork
    {
        Task<int> SaveAsync(CancellationToken cancellationToken = default);
    }
}
