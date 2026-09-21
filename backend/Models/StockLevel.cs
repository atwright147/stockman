using System;

namespace StockMan.Models
{
  public class StockLevel
  {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int LocationId { get; set; }
    public int QuantityOnHand { get; set; }
    public int QuantityAllocated { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
  }
}
