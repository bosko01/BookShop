using BookShop.Application.Interfaces.Payments;
using BookShop.Application.Interfaces.Persistence;
using BookShop.Application.Interfaces.Persistence.Common;
using BookShop.Application.Payment.Contracts;

namespace BookShop.Application.Payment.Services;

public sealed class PaymentWebhookService : IPaymentWebhookService
{
    private const string CardPaymentMethodName = "Card";

    private readonly IStripeWebhookEventParser _stripeWebhookEventParser;
    private readonly IOrderRepository _orderRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentWebhookService(
        IStripeWebhookEventParser stripeWebhookEventParser,
        IOrderRepository orderRepository,
        IInvoiceRepository invoiceRepository,
        IPaymentMethodRepository paymentMethodRepository,
        IUnitOfWork unitOfWork)
    {
        _stripeWebhookEventParser = stripeWebhookEventParser;
        _orderRepository = orderRepository;
        _invoiceRepository = invoiceRepository;
        _paymentMethodRepository = paymentMethodRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleStripeWebhookAsync(string payload, string signatureHeader, CancellationToken cancellationToken = default)
    {
        var eventData = _stripeWebhookEventParser.Parse(payload, signatureHeader);

        if (!eventData.IsSuccessfulPaymentEvent || eventData.OrderId is null)
            return;

        await Task.Delay(2000, cancellationToken);

        var order = await _orderRepository.GetByIdAsync(eventData.OrderId.Value, cancellationToken);
        if (order is null)
            return;

        var invoice = await _invoiceRepository.GetByOrderIdAsync(order.Id, cancellationToken);
        if (invoice is null)
        {
            var paymentMethod = await _paymentMethodRepository.GetByNameAsync(CardPaymentMethodName, cancellationToken);
            if (paymentMethod is null)
                return;

            invoice = Domain.Entities.Invoice.Create(order.Id, paymentMethod.Id, order.TotalAmount, "Stripe");
            await _invoiceRepository.AddAsync(invoice, cancellationToken);
        }

        order.MarkAsPaid();
        invoice.MarkAsPaid(DateTime.UtcNow, eventData.ProviderReference);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}
