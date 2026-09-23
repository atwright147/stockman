namespace StockMan.Models;

public class Location
{
  public int Id { get; set; }
  public string Aisle { get; set; } = "";
  public string Shelf { get; set; } = "";
  public string Container { get; set; } = "";
  public string Code { get; set; } = "";
  public bool IsActive { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }

}
