namespace BookShop.Application.Invoice.Contracts.Response;

public sealed record InvoiceResponse(
    Guid Id,
    int OrderId,
    int PaymentMethodId,
    decimal Amount,
    bool IsPaid,
    DateTime IssuedAtUtc,
    DateTime? PaidAtUtc,
    string? Provider,
    string? ProviderReference);
