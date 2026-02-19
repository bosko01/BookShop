namespace BookShop.Application.Order.Contracts.Request;

public sealed record ChangeOrderItemQuantityRequest(int OrderItemId, int Quantity);
