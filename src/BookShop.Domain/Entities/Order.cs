using System.ComponentModel.DataAnnotations;
using Bookstore.Domain.Enums;

namespace BookShop.Domain.Entities;

public class Order
{
    private Order() { } // EF

    private Order(int userId)
    {
        UserId = userId;
        Status = OrderStatus.Created;
        CreatedAtUtc = DateTime.UtcNow;
    }

    [Key]
    public int Id { get; private set; }

    public int UserId { get; private set; }

    public OrderStatus Status { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public decimal TotalAmount { get; private set; }

    public User User { get; private set; } = default!;
    public ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();

    public Invoice? Invoice { get; private set; }

    public static Order Create(int userId)
    {
        if (userId <= 0)
            throw new ArgumentOutOfRangeException(nameof(userId));

        return new Order(userId);
    }

    public void AddItem(OrderItem item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        Items.Add(item);
        RecalculateTotal();
    }

    private void RecalculateTotal()
    {
        TotalAmount = Items.Sum(x => x.UnitPrice * x.Quantity);
    }

    public void MarkAsPaid()
    {
        if (Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Cancelled order cannot be paid.");

        Status = OrderStatus.Paid;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Paid)
            throw new InvalidOperationException("Paid order cannot be cancelled.");

        Status = OrderStatus.Cancelled;
    }

    public void MarkAsShipped()
    {
        if (Status != OrderStatus.Paid)
            throw new InvalidOperationException("Only paid orders can be shipped.");

        Status = OrderStatus.Shipped;
    }

    public void MarkAsDelivered()
    {
        if (Status != OrderStatus.Shipped)
            throw new InvalidOperationException("Only shipped orders can be delivered.");

        Status = OrderStatus.Delivered;
    }
}