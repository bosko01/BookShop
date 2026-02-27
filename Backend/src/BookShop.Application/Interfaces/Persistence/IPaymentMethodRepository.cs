using BookShop.Application.Interfaces.Persistence.Common;
using BookShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using PaymentMethodEntity = BookShop.Domain.Entities.PaymentMethod;

namespace BookShop.Application.Interfaces.Persistence
{
    public interface IPaymentMethodRepository : IRepository<PaymentMethodEntity, int>
    {
        Task<PaymentMethodEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
