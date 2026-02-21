namespace BookShop.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Author
{
	private Author() { } // EF Core

	private Author(string firstName, string lastName, string? biography)
	{
		FirstName = firstName;
		LastName = lastName;
		Biography = biography;
	}

	[Key]
	public int Id { get; private set; }

	[Required, MaxLength(100)]
	public string FirstName { get; private set; } = default!;

	[Required, MaxLength(100)]
	public string LastName { get; private set; } = default!;

	[MaxLength(2000)]
	public string? Biography { get; private set; }

	public ICollection<Book> Books { get; private set; } = new List<Book>();

	public static Author Create(string firstName, string lastName, string? biography = null)
	{
		if (string.IsNullOrWhiteSpace(firstName))
			throw new ArgumentException("First name is required.", nameof(firstName));

		if (string.IsNullOrWhiteSpace(lastName))
			throw new ArgumentException("Last name is required.", nameof(lastName));

		if (firstName.Length > 100)
			throw new ArgumentException("First name is too long.", nameof(firstName));

		if (lastName.Length > 100)
			throw new ArgumentException("Last name is too long.", nameof(lastName));

		if (biography is not null && biography.Length > 2000)
			throw new ArgumentException("Biography is too long.", nameof(biography));

		return new Author(firstName.Trim(), lastName.Trim(), biography);
	}

	public void Rename(string firstName, string lastName)
	{
		if (string.IsNullOrWhiteSpace(firstName))
			throw new ArgumentException("First name is required.", nameof(firstName));

		if (string.IsNullOrWhiteSpace(lastName))
			throw new ArgumentException("Last name is required.", nameof(lastName));

		if (firstName.Length > 100)
			throw new ArgumentException("First name is too long.", nameof(firstName));

		if (lastName.Length > 100)
			throw new ArgumentException("Last name is too long.", nameof(lastName));

		FirstName = firstName.Trim();
		LastName = lastName.Trim();
	}

	public void UpdateBiography(string? biography)
	{
		if (biography is not null && biography.Length > 2000)
			throw new ArgumentException("Biography is too long.", nameof(biography));

		Biography = biography;
	}

    public string GetFullName() => $"{FirstName} {LastName}";
}