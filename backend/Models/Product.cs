using System;

namespace StockMan.Models
{
  public class Product
  {
    public int Id { get; set; }
    public string Sku { get; set; } = "";
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public int ReorderThreshold { get; set; }
    public int MinimumReorderQuantity { get; set; }
    public decimal UnitCost { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
  }
}
