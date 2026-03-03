using BookShop.Application.Interfaces.Persistence.Common;
using BookShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using OrderEntity = BookShop.Domain.Entities.Order;

namespace BookShop.Application.Interfaces.Persistence
{
    public interface IOrderRepository : IRepository<OrderEntity, int>
    {
        Task<OrderEntity?> GetByIdWithItemsAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<OrderEntity>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);

    }
}
