using ChicStreetwear.Application.Common.Interfaces;
using ChicStreetwear.Domain.Aggregates.Cart;
using ChicStreetwear.Domain.Aggregates.Category;
using ChicStreetwear.Domain.Aggregates.Order;
using ChicStreetwear.Domain.Aggregates.Product;
using ChicStreetwear.Domain.Aggregates.ProductReview;
using ChicStreetwear.Domain.Aggregates.User;
using ChicStreetwear.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ChicStreetwear.Infrastructure.Persistence;
/*
 * command to add migration:
 * dotnet ef migrations add <migration-name> -c ChicStreetwearDbContext -p ChicStreetwear.Infrastructure -s ChicStreetwear.Server -o Persistence/Migrations
 * command to update database:
 * dotnet ef database update -c ChicStreetwearDbContext -p ChicStreetwear.Infrastructure -s ChicStreetwear.Server
 */
public class ChicStreetwearDbContext : DbContext
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<ProductReview> ProductReviews { get; set; }
    public DbSet<User> Users { get; set; }
    public ChicStreetwearDbContext(DbContextOptions<ChicStreetwearDbContext> options, IDomainEventDispatcher domainEventDispatcher) : base(options)
    {
        _domainEventDispatcher = domainEventDispatcher;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entities = ChangeTracker
            .Entries<EntityBase>()
            .Where(entrie => entrie.Entity.DomainEvents.Any())
            .Select(entrie => entrie.Entity);

        await _domainEventDispatcher.PublishDomainEvents(entities);

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
