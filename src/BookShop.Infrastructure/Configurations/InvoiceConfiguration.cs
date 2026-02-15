using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShopq.Infrastructure.Persistence.Configurations;

public sealed class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount).HasColumnType("decimal(18,2)");
        builder.Property(x => x.IsPaid).HasDefaultValue(false);
        builder.Property(x => x.IssuedAtUtc).IsRequired();

        builder.Property(x => x.Provider).HasMaxLength(100);
        builder.Property(x => x.ProviderReference).HasMaxLength(200);

        // 1:0..1 with Order (enforced by unique FK)
        builder.HasIndex(x => x.OrderId).IsUnique();

        builder.HasOne(x => x.Order)
               .WithOne(x => x.Invoice)
               .HasForeignKey<Invoice>(x => x.OrderId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.PaymentMethod)
               .WithMany()
               .HasForeignKey(x => x.PaymentMethodId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}