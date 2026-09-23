using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockMan.Models;

namespace StockMan.Data.Configurations
{
  public class ProductConfiguration : IEntityTypeConfiguration<Product>
  {
    public void Configure(EntityTypeBuilder<Product> builder)
    {
      builder.HasKey(entity => entity.Id);
      builder.HasIndex(entity => entity.Sku).IsUnique();
      builder.HasIndex(entity => entity.Barcode).IsUnique();
    }
  }
}
