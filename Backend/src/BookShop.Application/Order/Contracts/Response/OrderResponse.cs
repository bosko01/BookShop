using Bookstore.Domain.Enums;

namespace BookShop.Application.Order.Contracts.Response;

public sealed record OrderResponse(
    int Id,
    int UserId,
    decimal TotalAmount,
    OrderStatus Status,
    DateTime CreatedAtUtc,
    int ItemCount,
    IReadOnlyList<OrderItemResponse> Items);

public sealed record OrderItemResponse(int OrderItemId, int BookId, string BookTitle, int Quantity, decimal UnitPrice);
