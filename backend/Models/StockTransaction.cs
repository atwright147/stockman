using System.ComponentModel.DataAnnotations;
using StockMan.Contracts;

namespace StockMan.Models;

public class StockTransaction
{
  public int Id { get; set; }

  public int ProductId { get; set; }
  public Product Product { get; set; } = null!;

  public int? FromLocationId { get; set; }
  public Location? FromLocation { get; set; }

  public int? ToLocationId { get; set; }
  public Location? ToLocation { get; set; }

  public int Quantity { get; set; }
  public TransactionType Type { get; set; }

  [Required, MaxLength(250)]
  public string Reason { get; set; } = string.Empty;

  public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
  public string CreatedByUserId { get; set; } = string.Empty;
}
