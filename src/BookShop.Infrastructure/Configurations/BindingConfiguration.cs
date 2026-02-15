using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistence.Configurations;

public sealed class BindingConfiguration : IEntityTypeConfiguration<Binding>
{
    public void Configure(EntityTypeBuilder<Binding> builder)
    {
        builder.ToTable("Bindings");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

        builder.HasMany(x => x.Books)
               .WithOne(x => x.Binding)
               .HasForeignKey(x => x.BindingId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
