using BookShop.Application.Interfaces.Persistence.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Infrastructure.Persistence.Common
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly BookShopDbContext _dbContext;

        public UnitOfWork(BookShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<int> SaveAsync(CancellationToken cancellationToken = default)
            => _dbContext.SaveChangesAsync(cancellationToken);
    }
}
