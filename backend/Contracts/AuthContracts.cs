namespace StockMan.Contracts
{
  public record RegisterRequest(string Email, string Password, string FirstName, string LastName);

  public record LoginRequest(string Email, string Password);

  public record AuthUserResponse(string Id, string Email, string FirstName, string LastName, IEnumerable<string> Roles);

  public record LoginResponse(string Token, DateTime ExpiresAt, AuthUserResponse User);
}
