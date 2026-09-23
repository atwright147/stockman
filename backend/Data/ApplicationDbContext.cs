using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StockMan.Entities;
using StockMan.Models;

namespace StockMan.Data
{
  public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
  {
    public DbSet<Product> Products { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<StockLevel> StockLevel { get; set; }
    public DbSet<StockTransaction> StockTransaction { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      foreach (var entry in ChangeTracker.Entries<StockLevel>()
        .Where(e => e.State is EntityState.Added or EntityState.Modified))
      {
        entry.Entity.RowVersion = Guid.NewGuid().ToByteArray();
      }

      return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
  }
}
