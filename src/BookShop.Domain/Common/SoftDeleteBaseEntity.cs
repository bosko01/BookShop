namespace BookShop.Domain.Common;

public abstract class SoftDeleteBaseEntity : BaseEntity
{
    public bool IsDeleted { get; protected set; }

    public DateTime? DeletedAtUtc { get; protected set; }

    public void MarkAsDeleted(DateTime utcNow)
    {
        if (IsDeleted) return;

        IsDeleted = true;
        DeletedAtUtc = utcNow;
    }
}
