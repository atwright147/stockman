using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StockMan.Entities;
using StockMan.Models;

namespace StockMan.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
  : IdentityDbContext<ApplicationUser>(options)
{
  public DbSet<Product> Products { get; set; } = null!;
  public DbSet<Location> Locations { get; set; } = null!;
  public DbSet<StockLevel> StockLevel { get; set; } = null!;
  public DbSet<StockTransaction> StockTransaction { get; set; } = null!;

  public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    foreach (var entry in ChangeTracker.Entries<StockLevel>()
      .Where(e => e.State is EntityState.Added or EntityState.Modified))
    {
      entry.Entity.RowVersion = Guid.NewGuid().ToByteArray();
    }

    return base.SaveChangesAsync(cancellationToken);
  }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);
    builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
  }
}
