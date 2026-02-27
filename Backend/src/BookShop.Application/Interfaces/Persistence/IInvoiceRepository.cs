using BookShop.Application.Interfaces.Persistence.Common;
using System;
using System.Collections.Generic;
using System.Text;
using InvoiceEntity = BookShop.Domain.Entities.Invoice;

namespace BookShop.Application.Interfaces.Persistence
{
    public interface IInvoiceRepository : IRepository<InvoiceEntity, Guid>
    {
        Task<InvoiceEntity?> GetByOrderIdAsync(int orderId, CancellationToken cancellationToken = default);
    }
}
