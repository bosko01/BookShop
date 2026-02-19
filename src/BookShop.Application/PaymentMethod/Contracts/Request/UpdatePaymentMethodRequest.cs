namespace BookShop.Application.PaymentMethod.Contracts.Request;

public sealed record UpdatePaymentMethodRequest(string Name, string? Description);
