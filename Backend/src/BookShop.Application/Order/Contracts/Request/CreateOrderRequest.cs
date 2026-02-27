namespace BookShop.Application.Order.Contracts.Request;

public sealed record CreateOrderRequest(int UserId, IReadOnlyList<CreateOrderItemRequest> Items);

public sealed record CreateOrderItemRequest(int BookId, int Quantity);
