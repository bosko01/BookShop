using BookShop.Application.Common.Exceptions;
using BookShop.Application.Interfaces.Persistence;
using BookShop.Application.Interfaces.Persistence.Common;
using BookShop.Application.Invoice.Contracts;
using BookShop.Application.Invoice.Contracts.Request;
using BookShop.Application.Invoice.Contracts.Response;

namespace BookShop.Application.Invoice.Services;

public sealed class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    private readonly IUnitOfWork _unitOfWork;

    public InvoiceService(IInvoiceRepository invoiceRepository, IOrderRepository orderRepository, IPaymentMethodRepository paymentMethodRepository, IUnitOfWork unitOfWork)
    {
        _invoiceRepository = invoiceRepository;
        _orderRepository = orderRepository;
        _paymentMethodRepository = paymentMethodRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<InvoiceResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        => (await _invoiceRepository.GetAllAsync(cancellationToken)).Select(Map).ToList();

    public async Task<InvoiceResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("Invoice", id);
        return Map(invoice);
    }

    public async Task<Guid> CreateAsync(CreateInvoiceRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _orderRepository.ExistsAsync(request.OrderId, cancellationToken))
            throw new NotFoundException("Order", request.OrderId);
        if (!await _paymentMethodRepository.ExistsAsync(request.PaymentMethodId, cancellationToken))
            throw new NotFoundException("PaymentMethod", request.PaymentMethodId);

        var invoice = Domain.Entities.Invoice.Create(request.OrderId, request.PaymentMethodId, request.Amount, request.Provider);
        await _invoiceRepository.AddAsync(invoice, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        return invoice.Id;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("Invoice", id);
        _invoiceRepository.Remove(invoice);
        await _unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task MarkAsPaidAsync(Guid id, MarkInvoicePaidRequest request, CancellationToken cancellationToken = default)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("Invoice", id);
        invoice.MarkAsPaid(DateTime.UtcNow, request.ProviderReference);
        await _unitOfWork.SaveAsync(cancellationToken);
    }

    private static InvoiceResponse Map(Domain.Entities.Invoice invoice)
        => new(invoice.Id, invoice.OrderId, invoice.PaymentMethodId, invoice.Amount, invoice.IsPaid, invoice.IssuedAtUtc, invoice.PaidAtUtc, invoice.Provider, invoice.ProviderReference);
}
