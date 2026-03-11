using BookShop.Application.Interfaces.Payments;
using BookShop.Application.Interfaces.Persistence;
using BookShop.Application.Interfaces.Persistence.Common;
using BookShop.Application.Payment.Contracts;
using Microsoft.Extensions.Logging;

namespace BookShop.Application.Payment.Services;

public sealed class PaymentWebhookService : IPaymentWebhookService
{
    private const string CardPaymentMethodName = "Card";
    private readonly IStripeWebhookEventParser _stripeWebhookEventParser;
    private readonly IOrderRepository _orderRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PaymentWebhookService> _logger;

    public PaymentWebhookService(
        IStripeWebhookEventParser stripeWebhookEventParser,
        IOrderRepository orderRepository,
        IInvoiceRepository invoiceRepository,
        IPaymentMethodRepository paymentMethodRepository,
        IUnitOfWork unitOfWork,
        ILogger<PaymentWebhookService> logger)
    {
        _stripeWebhookEventParser = stripeWebhookEventParser;
        _orderRepository = orderRepository;
        _invoiceRepository = invoiceRepository;
        _paymentMethodRepository = paymentMethodRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task HandleStripeWebhookAsync(string payload, string signatureHeader, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Stripe webhook processing started.");

        var eventData = _stripeWebhookEventParser.Parse(payload, signatureHeader);
        _logger.LogInformation("Stripe webhook event parsed. EventType={EventType}.", eventData.EventType);

        if (!eventData.IsCheckoutSessionCompleted)
        {
            _logger.LogInformation("Ignoring Stripe event {EventType}. Only checkout.session.completed is processed.", eventData.EventType);
            return;
        }

        if (eventData.OrderId is null)
        {
            _logger.LogWarning("checkout.session.completed event does not contain order id. EventType={EventType}", eventData.EventType);
            return;
        }

        _logger.LogInformation("Order lookup started. OrderId={OrderId}.", eventData.OrderId.Value);
        var order = await _orderRepository.GetByIdWithItemsAsync(eventData.OrderId.Value, cancellationToken);
        if (order is null)
        {
            _logger.LogWarning("Order not found during webhook processing. OrderId={OrderId}. Webhook will return success to avoid Stripe timeout.", eventData.OrderId.Value);
            return;
        }

        _logger.LogInformation("Order found. OrderId={OrderId}. Starting invoice processing.", order.Id);

        var invoice = await _invoiceRepository.GetByOrderIdAsync(order.Id, cancellationToken);
        if (invoice is null)
        {
            var paymentMethod = await _paymentMethodRepository.GetByNameAsync(CardPaymentMethodName, cancellationToken);
            if (paymentMethod is null)
            {
                _logger.LogWarning("Payment method '{PaymentMethodName}' was not found. Invoice generation skipped for OrderId={OrderId}.", CardPaymentMethodName, order.Id);
                return;
            }

            _logger.LogInformation("Invoice generation started. OrderId={OrderId}.", order.Id);
            invoice = Domain.Entities.Invoice.Create(order.Id, paymentMethod.Id, order.TotalAmount, "Stripe");
            await _invoiceRepository.AddAsync(invoice, cancellationToken);
        }

        if (invoice.IsPaid)
        {
            _logger.LogInformation("Invoice is already paid. Duplicate webhook ignored. OrderId={OrderId}, InvoiceId={InvoiceId}.", order.Id, invoice.Id);
            return;
        }

        foreach (var item in order.Items)
        {
            item.Book.DecrementStock(item.Quantity);
        }

        order.MarkAsPaid();
        invoice.MarkAsPaid(DateTime.UtcNow, eventData.ProviderReference);
        await _unitOfWork.SaveAsync(cancellationToken);

        _logger.LogInformation("Stripe webhook processing finished. OrderId={OrderId}.", order.Id);
    }
}
