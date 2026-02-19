namespace BookShop.Application.PaymentMethod.Contracts.Request;

public sealed record CreatePaymentMethodRequest(string Name, string? Description);
