namespace StockMan.Contracts;

public record ProductCreateRequest(
  string Sku,
  string Name,
  string? Description,
  string? Barcode,
  int ReorderThreshold,
  int MinimumReorderQuantity,
  decimal UnitCost
);

public record ProductUpdateRequest(
  string? Sku,
  string? Name,
  string? Description,
  string? Barcode,
  int? ReorderThreshold,
  int? MinimumReorderQuantity,
  decimal? UnitCost
);

public record ProductResponse(
  int Id,
  string Sku,
  string Name,
  string? Description,
  string? Barcode,
  int ReorderThreshold,
  int MinimumReorderQuantity,
  decimal UnitCost,
  DateTime CreatedAt,
  DateTime? UpdatedAt
);
