namespace BookShop.Application.Invoice.Contracts.Request;

public sealed record CreateInvoiceRequest(int OrderId, int PaymentMethodId, decimal Amount, string? Provider);
