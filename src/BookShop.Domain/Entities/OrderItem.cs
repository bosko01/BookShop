using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Domain.Entities;

public class OrderItem
{
	private OrderItem() { } // EF

	private OrderItem(int bookId, int quantity, decimal unitPrice)
	{
		BookId = bookId;
		Quantity = quantity;
		UnitPrice = unitPrice;
	}

	[Key]
	public int Id { get; private set; }

	public int OrderId { get; private set; }

	public int BookId { get; private set; }

	public int Quantity { get; private set; }

	[Column(TypeName = "decimal(18,2)")]
	public decimal UnitPrice { get; private set; }

	public Order Order { get; private set; } = default!;
	public Book Book { get; private set; } = default!;

	public static OrderItem Create(int bookId, int quantity, decimal unitPrice)
	{
		if (bookId <= 0)
			throw new ArgumentOutOfRangeException(nameof(bookId));

		if (quantity <= 0)
			throw new ArgumentOutOfRangeException(nameof(quantity));

		if (unitPrice <= 0)
			throw new ArgumentOutOfRangeException(nameof(unitPrice));

		return new OrderItem(bookId, quantity, unitPrice);
	}

	public void ChangeQuantity(int quantity)
	{
		if (quantity <= 0)
			throw new ArgumentOutOfRangeException(nameof(quantity));

		Quantity = quantity;
	}
}