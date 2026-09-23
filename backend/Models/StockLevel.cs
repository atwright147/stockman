using System;
using System.ComponentModel.DataAnnotations;

namespace StockMan.Models
{
  public class StockLevel
  {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int LocationId { get; set; }
    public Location Location { get; set; } = null!;
    public int QuantityOnHand { get; set; }
    public int QuantityAllocated { get; set; }
    public int QuantityAvailable => QuantityOnHand - QuantityAllocated;
    [Timestamp]
    public byte[] RowVersion { get; set; } = null!;
  }
}
