using BookShop.Application.Interfaces.Payments;
using BookShop.Application.Interfaces.Persistence;
using BookShop.Application.Interfaces.Persistence.Common;
using BookShop.Application.Payment.Contracts;

namespace BookShop.Application.Payment.Services;

public sealed class PaymentWebhookService : IPaymentWebhookService
{
    private readonly IStripeWebhookEventParser _stripeWebhookEventParser;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentWebhookService(
        IStripeWebhookEventParser stripeWebhookEventParser,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _stripeWebhookEventParser = stripeWebhookEventParser;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleStripeWebhookAsync(string payload, string signatureHeader, CancellationToken cancellationToken = default)
    {
        var eventData = _stripeWebhookEventParser.Parse(payload, signatureHeader);

        if (!eventData.IsSuccessfulPaymentEvent || eventData.OrderId is null)
            return;

        var order = await _orderRepository.GetByIdAsync(eventData.OrderId.Value, cancellationToken);
        if (order is null)
            return;

        order.MarkAsPaid();
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}
