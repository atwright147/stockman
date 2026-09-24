using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockMan.Models;

namespace StockMan.Data.Configurations;

public class StockLevelConfiguration : IEntityTypeConfiguration<StockLevel>
{
  public void Configure(EntityTypeBuilder<StockLevel> builder)
  {
    builder.HasKey(entity => entity.Id);

    builder.HasIndex(entity => new
    {
      entity.ProductId,
      entity.LocationId
    }).IsUnique();

    builder.HasOne(entity => entity.Product)
      .WithMany()
      .HasForeignKey(entity => entity.ProductId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(entity => entity.Location)
      .WithMany()
      .HasForeignKey(entity => entity.LocationId)
      .OnDelete(DeleteBehavior.Restrict);

    // Computed column ignored by EF migrations
    builder.Ignore(entity => entity.QuantityAvailable);

    // SQLite can't generate rowversion values, so the concurrency token
    // is supplied in code (see ApplicationDbContext.SaveChangesAsync).
    builder.Property(entity => entity.RowVersion)
      .IsConcurrencyToken()
      .ValueGeneratedNever();
  }
}
