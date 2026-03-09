using BookShop.Application.Interfaces.Payments;
using BookShop.Application.Interfaces.Persistence;
using BookShop.Application.Interfaces.Persistence.Common;
using BookShop.Application.Payment.Contracts;

namespace BookShop.Application.Payment.Services;

public sealed class PaymentWebhookService : IPaymentWebhookService
{
    private const string CardPaymentMethodName = "Card";
    private static readonly TimeSpan[] OrderLookupRetryDelays =
    [
        TimeSpan.FromMilliseconds(250),
        TimeSpan.FromMilliseconds(500),
        TimeSpan.FromSeconds(1),
        TimeSpan.FromSeconds(2),
        TimeSpan.FromSeconds(3)
    ];

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

        var order = await WaitForOrderAsync(eventData.OrderId.Value, cancellationToken);
        if (order is null)
            throw new InvalidOperationException($"Order with id '{eventData.OrderId.Value}' is not available yet. Webhook processing will be retried.");

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

    private async Task<Domain.Entities.Order?> WaitForOrderAsync(int orderId, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
        if (order is not null)
            return order;

        foreach (var delay in OrderLookupRetryDelays)
        {
            await Task.Delay(delay, cancellationToken);

            order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
            if (order is not null)
                return order;
        }

        return null;
    }
}
