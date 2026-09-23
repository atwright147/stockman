using System;
using StockMan.Models;

namespace StockMan.Contracts
{
  public record StockLevelCreateRequest(
    int ProductId,
    int LocationId,
    int QuantityOnHand,
    int QuantityAllocated
  );

  public record StockLevelUpdateRequest(
    int? ProductId,
    int? LocationId,
    int? QuantityOnHand,
    int? QuantityAllocated
  );

  public record StockLevelResponse(
    int Id,
    int ProductId,
    int LocationId,
    int QuantityOnHand,
    int QuantityAllocated
  );
}
