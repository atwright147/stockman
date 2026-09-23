using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockMan.Models;

namespace StockMan.Data.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
  public void Configure(EntityTypeBuilder<Location> builder)
  {
    builder.HasKey(entity => entity.Id);
    builder.HasIndex(entity => entity.Code).IsUnique();
  }
}
