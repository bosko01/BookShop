namespace Bookstore.Domain.Enums
{
    public enum UserRole
    {
        User = 0,
        Admin = 1
    }

    public enum OrderStatus
    {
        Created = 0,
        Paid = 1,
        Cancelled = 2,
        Shipped = 3,
        Delivered = 4
    }
}