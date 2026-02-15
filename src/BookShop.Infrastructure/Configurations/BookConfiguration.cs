using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistence.Configurations;

public sealed class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).HasMaxLength(250).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(4000);
        builder.Property(x => x.ImageUrl).HasMaxLength(500);

        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
        builder.Property(x => x.QuantityInStock).IsRequired();

        builder.Property(x => x.IsDeleted).HasDefaultValue(false);
        builder.Property(x => x.DeletedAtUtc);

        builder.HasOne(x => x.Author)
               .WithMany(x => x.Books)
               .HasForeignKey(x => x.AuthorId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Publisher)
               .WithMany(x => x.Books)
               .HasForeignKey(x => x.PublisherId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Genre)
               .WithMany(x => x.Books)
               .HasForeignKey(x => x.GenreId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Binding)
               .WithMany(x => x.Books)
               .HasForeignKey(x => x.BindingId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
