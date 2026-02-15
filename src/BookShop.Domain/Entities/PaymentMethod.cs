using System.ComponentModel.DataAnnotations;

namespace BookShop.Domain.Entities;

public class PaymentMethod
{
    private PaymentMethod() { } // EF

    private PaymentMethod(string name, string? description)
    {
        Name = name;
        Description = description;
        IsActive = true;
    }

    [Key]
    public int Id { get; private set; }

    [Required, MaxLength(100)]
    public string Name { get; private set; } = default!;

    [MaxLength(250)]
    public string? Description { get; private set; }

    public bool IsActive { get; private set; }

    public static PaymentMethod Create(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        return new PaymentMethod(name.Trim(), description);
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        Name = name.Trim();
    }

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;
}