using ChicStreetwear.Application.Common.Interfaces;
using ChicStreetwear.Domain.Aggregates.Category;
using ChicStreetwear.Domain.Aggregates.Order;
using ChicStreetwear.Domain.Aggregates.Product;
using ChicStreetwear.Domain.Common;
using ChicStreetwear.Shared.Identity.Models;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace ChicStreetwear.Infrastructure.Identity;

/*
 * command to add migration:
 * dotnet ef migrations add <migration-name> -c ChicStreetwearIdentityDbContext -p ChicStreetwear.Infrastructure -s ChicStreetwear.Server -o Identity/Migrations
 * command to update database:
 * dotnet ef database update -c ChicStreetwearIdentityDbContext -p ChicStreetwear.Infrastructure -s ChicStreetwear.Server
 */

public class ChicStreetwearIdentityDbContext : ApiAuthorizationDbContextBase<ApplicationUser, ApplicationRole, Guid>
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }

    public ChicStreetwearIdentityDbContext(DbContextOptions<ChicStreetwearIdentityDbContext> options,
                                           IOptions<OperationalStoreOptions> operationalStoreOptions,
                                           IDomainEventDispatcher domainEventDispatcher)
        : base(options, operationalStoreOptions)
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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
