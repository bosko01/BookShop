using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistence.Configurations;

public sealed class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
{
	public void Configure(EntityTypeBuilder<Publisher> builder)
	{
		builder.ToTable("Publishers");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
		builder.Property(x => x.Address).HasMaxLength(200);
		builder.Property(x => x.City).HasMaxLength(100);
		builder.Property(x => x.Country).HasMaxLength(100);
		builder.Property(x => x.PhoneNumber).HasMaxLength(30);

		builder.HasMany(x => x.Books)
			   .WithOne(x => x.Publisher)
			   .HasForeignKey(x => x.PublisherId)
			   .OnDelete(DeleteBehavior.Restrict);
	}
}
