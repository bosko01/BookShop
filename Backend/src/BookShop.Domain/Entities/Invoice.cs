using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Domain.Entities;

public class Invoice
{
    private Invoice() { } // EF

    private Invoice(int orderId, int paymentMethodId, decimal amount, string? provider)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        PaymentMethodId = paymentMethodId;
        Amount = amount;
        Provider = provider;
        IssuedAtUtc = DateTime.UtcNow;
    }

    [Key]
    public Guid Id { get; private set; }

    public int OrderId { get; private set; }

    public int PaymentMethodId { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; private set; }

    public bool IsPaid { get; private set; }

    public DateTime IssuedAtUtc { get; private set; }

    public DateTime? PaidAtUtc { get; private set; }

    [MaxLength(100)]
    public string? Provider { get; private set; }

    [MaxLength(200)]
    public string? ProviderReference { get; private set; }

    public Order Order { get; private set; } = default!;
    public PaymentMethod PaymentMethod { get; private set; } = default!;

    public static Invoice Create(
        int orderId,
        int paymentMethodId,
        decimal amount,
        string? provider = null)
    {
        if (orderId <= 0)
            throw new ArgumentOutOfRangeException(nameof(orderId));

        if (paymentMethodId <= 0)
            throw new ArgumentOutOfRangeException(nameof(paymentMethodId));

        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount));

        return new Invoice(orderId, paymentMethodId, amount, provider);
    }

    public void MarkAsPaid(DateTime utcNow, string? providerReference = null)
    {
        if (IsPaid) return;

        IsPaid = true;
        PaidAtUtc = utcNow;

        if (!string.IsNullOrWhiteSpace(providerReference))
            ProviderReference = providerReference;
    }
}