namespace BookShop.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; protected set; }
    public DateTime CreatedAtUtc { get; protected set; } = DateTime.UtcNow;

    protected BaseEntity() 
    {
        CreatedAtUtc = DateTime.UtcNow;
    }
}