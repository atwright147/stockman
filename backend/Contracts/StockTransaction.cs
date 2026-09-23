using StockMan.Models;


namespace StockMan.Contracts
{

  public enum TransactionType
  {
    GoodsIn,
    GoodsOut,
    Transfer,
    Adjustment
  }

  public record StockTransactionCreateRequest(
    int ProductId,
    int? FromLocationId,
    int? ToLocationId,
    int Quantity,
    TransactionType Type,
    string Reason,
    string CreatedByUserId
  );

  public record StockTransactionUpdateRequest(
    int? ProductId,
    int? FromLocationId,
    int? ToLocationId,
    int? Quantity,
    TransactionType? Type,
    string? Reason,
    string? CreatedByUserId
  );

  public record StockTransactionResponse(
    int Id,
    int ProductId,
    Product Product,
    int? FromLocationId,
    Location? FromLocation,
    int? ToLocationId,
    Location? ToLocation,
    int Quantity,
    TransactionType Type,
    string Reason,
    DateTime TimestampUtc,
    string CreatedByUserId
  );
}
