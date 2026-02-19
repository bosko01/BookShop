using System.ComponentModel.DataAnnotations;
using Bookstore.Domain.Enums;

namespace BookShop.Domain.Entities;

public class User
{
    private User() { }

    private User(
        string firstName,
        string lastName,
        string email,
        string passwordHash,
        UserRole role)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        CreatedAtUtc = DateTime.UtcNow;
    }

    [Key]
    public int Id { get; private set; }

    [Required, MaxLength(100)]
    public string FirstName { get; private set; } = default!;

    [Required, MaxLength(100)]
    public string LastName { get; private set; } = default!;

    [Required, EmailAddress, MaxLength(256)]
    public string Email { get; private set; } = default!;

    [Required]
    public string PasswordHash { get; private set; } = default!;

    public UserRole Role { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAtUtc { get; private set; }

    public ICollection<Order> Orders { get; private set; } = new List<Order>();
    public ICollection<Review> Reviews { get; private set; } = new List<Review>();

    public static User Create(
        string firstName,
        string lastName,
        string email,
        string passwordHash,
        UserRole role = UserRole.User)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required.", nameof(lastName));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required.", nameof(passwordHash));

        return new User(
            firstName.Trim(),
            lastName.Trim(),
            email.Trim(),
            passwordHash,
            role);
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("Password hash is required.", nameof(newPasswordHash));

        PasswordHash = newPasswordHash;
    }

    public void UpdateProfile(string firstName, string lastName, string email)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required.", nameof(lastName));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Email = email.Trim();
    }

    public void SetRole(UserRole role)
    {
        Role = role;
    }

    public void MarkAsDeleted(DateTime utcNow)
    {
        if (IsDeleted) return;

        IsDeleted = true;
        DeletedAtUtc = utcNow;
    }
}
