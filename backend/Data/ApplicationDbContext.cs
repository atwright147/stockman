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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<StockLevel>(entity =>
      {
        entity.ToTable("StockLevels");

        entity.HasOne<Product>()
          .WithMany()
          .HasForeignKey(sl => sl.ProductId)
          .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<Location>()
          .WithMany()
          .HasForeignKey(sl => sl.LocationId)
          .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(sl => new { sl.ProductId, sl.LocationId }).IsUnique();
      });
    }
  }
}
