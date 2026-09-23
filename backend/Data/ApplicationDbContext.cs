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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // Product Constraints
      modelBuilder.Entity<Product>(entity =>
      {
        entity.HasIndex(p => p.Sku).IsUnique();
        entity.HasIndex(p => p.Barcode).IsUnique();
      });

      // Location Constraints
      modelBuilder.Entity<Location>(entity => entity.HasIndex(l => l.Code).IsUnique());

      // StockLevel Composite Unique Index (One location can only have one entry per product)
      modelBuilder.Entity<StockLevel>(entity =>
      {
        entity.HasIndex(sl => new { sl.ProductId, sl.LocationId }).IsUnique();

        // Computed column ignored by EF migrations
        entity.Ignore(sl => sl.QuantityAvailable);
      });

      // StockTransaction Relationships
      modelBuilder.Entity<StockTransaction>(entity =>
      {
        entity.HasOne(st => st.FromLocation)
                .WithMany()
                .HasForeignKey(st => st.FromLocationId)
                .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(st => st.ToLocation)
                .WithMany()
                .HasForeignKey(st => st.ToLocationId)
                .OnDelete(DeleteBehavior.Restrict);
      });
    }
  }
}

/*
        // Product Constraints
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasIndex(p => p.SKU).IsUnique();
            entity.HasIndex(p => p.Barcode).IsUnique();
        });

        // Location Constraints
        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasIndex(l => l.Code).IsUnique();
        });

        // StockLevel Composite Unique Index (One location can only have one entry per product)
        modelBuilder.Entity<StockLevel>(entity =>
        {
            entity.HasIndex(sl => new { sl.ProductId, sl.LocationId }).IsUnique();

            // Computed column ignored by EF migrations
            entity.Ignore(sl => sl.QuantityAvailable);
        });

        // StockTransaction Relationships
        modelBuilder.Entity<StockTransaction>(entity =>
        {
            entity.HasOne(st => st.FromLocation)
                  .WithMany()
                  .HasForeignKey(st => st.FromLocationId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(st => st.ToLocation)
                  .WithMany()
                  .HasForeignKey(st => st.ToLocationId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
*/
