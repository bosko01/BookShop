using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace BookShop.Domain.Entities;

public class Binding
{
    private Binding() { }

    private Binding(string name)
    {
        Name = name;
    }

    [Key]
    public int Id { get; private set; }

    [Required, MaxLength(100)]
    public string Name { get; private set; } = default!;

    public ICollection<Book> Books { get; private set; } = new List<Book>();

    public static Binding Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("Name is too long.", nameof(name));

        return new Binding(name.Trim());
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("Name is too long.", nameof(name));

        Name = name.Trim();
    }
}