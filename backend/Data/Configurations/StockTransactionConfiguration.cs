using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockMan.Models;

namespace StockMan.Data.Configurations;

public class StockTransactionConfiguration : IEntityTypeConfiguration<StockTransaction>
{
  public void Configure(EntityTypeBuilder<StockTransaction> builder)
  {
    builder.HasKey(entity => entity.Id);

    builder.HasOne(entity => entity.FromLocation)
      .WithMany()
      .HasForeignKey(entity => entity.FromLocationId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(entity => entity.ToLocation)
      .WithMany()
      .HasForeignKey(entity => entity.ToLocationId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}
