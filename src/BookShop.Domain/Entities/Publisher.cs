namespace BookShop.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Publisher {

    private Publisher() { }

    private Publisher(string name,
        string? address,
        string? city,
        string? country,
        string? phoneNumber)
    {
        Name = name;
        Address = address;
        City = city;
        Country = country;
        PhoneNumber = phoneNumber;
    }

    [Key]
    public int Id { get; private set; }

    [Required, MaxLength(200)]
    public string Name { get; private set; } = default!;

    [MaxLength(200)]
    public string? Address { get; private set; }

    [MaxLength(100)]
    public string? City { get; private set; }

    [MaxLength(100)]
    public string? Country { get; private set; }

    [MaxLength(30)]
    public string? PhoneNumber { get; private set; }

    public ICollection<Book> Books { get; private set; } = new List<Book>();

    public static Publisher Create(
        string name,
        string? address = null,
        string? city = null,
        string? country = null,
        string? phoneNumber = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        if (name.Length > 200)
            throw new ArgumentException("Name is too long.", nameof(name));

        if (address is not null && address.Length > 200)
            throw new ArgumentException("Address is too long.", nameof(address));

        if (city is not null && city.Length > 100)
            throw new ArgumentException("City is too long.", nameof(city));

        if (country is not null && country.Length > 100)
            throw new ArgumentException("Country is too long.", nameof(country));

        if (phoneNumber is not null && phoneNumber.Length > 30)
            throw new ArgumentException("Phone number is too long.", nameof(phoneNumber));

        return new Publisher(name.Trim(), address, city, country, phoneNumber);
    }

    public void UpdateContactInfo(
        string? address,
        string? city,
        string? country,
        string? phoneNumber)
    {
        if (address is not null && address.Length > 200)
            throw new ArgumentException("Address is too long.", nameof(address));

        if (city is not null && city.Length > 100)
            throw new ArgumentException("City is too long.", nameof(city));

        if (country is not null && country.Length > 100)
            throw new ArgumentException("Country is too long.", nameof(country));

        if (phoneNumber is not null && phoneNumber.Length > 30)
            throw new ArgumentException("Phone number is too long.", nameof(phoneNumber));

        Address = address;
        City = city;
        Country = country;
        PhoneNumber = phoneNumber;
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        if (name.Length > 200)
            throw new ArgumentException("Name is too long.", nameof(name));

        Name = name.Trim();
    }
}