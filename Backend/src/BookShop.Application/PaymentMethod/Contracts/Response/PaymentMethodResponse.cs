namespace BookShop.Application.PaymentMethod.Contracts.Response;

public sealed record PaymentMethodResponse(int Id, string Name, string? Description, bool IsActive);
