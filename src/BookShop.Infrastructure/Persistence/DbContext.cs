using BookShop.Domain.Common;
using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace BookShop.Infrastructure.Persistence;

public sealed class BookShopDbContext : DbContext
{
	public BookShopDbContext(DbContextOptions<BookShopDbContext> options)
		: base(options)
	{
	}

	public DbSet<Author> Authors => Set<Author>();
	public DbSet<Binding> Bindings => Set<Binding>();
	public DbSet<Genre> Genres => Set<Genre>();
	public DbSet<Publisher> Publishers => Set<Publisher>();
	public DbSet<Book> Books => Set<Book>();

	public DbSet<User> Users => Set<User>();
	public DbSet<Review> Reviews => Set<Review>();

	public DbSet<Order> Orders => Set<Order>();
	public DbSet<OrderItem> OrderItems => Set<OrderItem>();

	public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
	public DbSet<Invoice> Invoices => Set<Invoice>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// Fluent configurations
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookShopDbContext).Assembly);

		// Global query filter for all soft-deletable entities
		ApplySoftDeleteQueryFilters(modelBuilder);
	}

	public override int SaveChanges()
	{
		ApplySoftDeleteBehavior();
		return base.SaveChanges();
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		ApplySoftDeleteBehavior();
		return base.SaveChangesAsync(cancellationToken);
	}

	private void ApplySoftDeleteBehavior()
	{
		var utcNow = DateTime.UtcNow;

		foreach (var entry in ChangeTracker.Entries<SoftDeleteBaseEntity>())
		{
			if (entry.State == EntityState.Deleted)
			{
				// Convert physical delete into soft delete
				entry.State = EntityState.Modified;
				entry.Entity.MarkAsDeleted(utcNow);
			}
		}
	}

	private static void ApplySoftDeleteQueryFilters(ModelBuilder modelBuilder)
	{
		// Apply filter to all entities that inherit SoftDeletableEntity
		foreach (var entityType in modelBuilder.Model.GetEntityTypes())
		{
			if (!typeof(SoftDeleteBaseEntity).IsAssignableFrom(entityType.ClrType))
				continue;

			var parameter = Expression.Parameter(entityType.ClrType, "e");
			var isDeletedProperty = Expression.Property(parameter, nameof(SoftDeleteBaseEntity.IsDeleted));
			var compareExpression = Expression.Equal(isDeletedProperty, Expression.Constant(false));

			var lambda = Expression.Lambda(compareExpression, parameter);
			modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
		}
	}
}