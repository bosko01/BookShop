using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistence.Configurations;

public sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Rating).IsRequired();
        builder.Property(x => x.Comment).HasMaxLength(2000);

        builder.Property(x => x.CreatedAtUtc).IsRequired();

        // 1 user -> 1 review per book
        builder.HasIndex(x => new { x.UserId, x.BookId }).IsUnique();

        // Soft delete
        builder.Property(x => x.IsDeleted).HasDefaultValue(false);
        builder.Property(x => x.DeletedAtUtc);

        builder.HasOne(x => x.Book)
               .WithMany(x => x.Reviews)
               .HasForeignKey(x => x.BookId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.User)
               .WithMany(x => x.Reviews)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}