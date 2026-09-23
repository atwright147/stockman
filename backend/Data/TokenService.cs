using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using StockMan.Entities;
using StockMan.Models;

namespace StockMan.Data;

public class TokenService(JwtSettings jwtSettings)
{
  public string CreateToken(ApplicationUser user, IList<string> roles)
  {
    var claims = new List<Claim>
    {
      new(ClaimTypes.NameIdentifier, user.Id),
      new(ClaimTypes.Name, user.UserName ?? string.Empty),
      new(ClaimTypes.Email, user.Email ?? string.Empty),
    };

    claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var expires = DateTime.UtcNow.AddMinutes(jwtSettings.ExpiryMinutes);

    var token = new JwtSecurityToken(
      issuer: jwtSettings.Issuer,
      audience: jwtSettings.Audience,
      claims: claims,
      expires: expires,
      signingCredentials: credentials);

    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}
