using BookShop.Application.Invoice.Contracts.Request;
using BookShop.Application.Invoice.Contracts.Response;

namespace BookShop.Application.Invoice.Contracts;

public interface IInvoiceService
{
    Task<IReadOnlyList<InvoiceResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<InvoiceResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(CreateInvoiceRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task MarkAsPaidAsync(Guid id, MarkInvoicePaidRequest request, CancellationToken cancellationToken = default);
}
