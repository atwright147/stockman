namespace StockMan.Contracts;

public record LocationCreateRequest(
  string Aisle,
  string Shelf,
  string Container,
  string Code,
  bool IsActive
);

public record LocationUpdateRequest(
  string? Aisle,
  string? Shelf,
  string? Container,
  string? Code,
  bool? IsActive
);

public record LocationResponse(
  int Id,
  string Aisle,
  string Shelf,
  string Container,
  string Code,
  bool IsActive,
  DateTime CreatedAt,
  DateTime? UpdatedAt
);
