using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StockMan.Contracts;
using StockMan.Data;
using StockMan.Entities;
using StockMan.Models;

namespace StockMan.Controllers
{
  [Route("api/auth")]
  [ApiController]
  public class AuthController(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    TokenService tokenService,
    IOptions<JwtSettings> jwtOptions) : ControllerBase
  {
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
      var user = new ApplicationUser
      {
        UserName = request.Email,
        Email = request.Email,
        FirstName = request.FirstName,
        LastName = request.LastName,
      };

      var result = await userManager.CreateAsync(user, request.Password);

      if (!result.Succeeded)
      {
        foreach (var error in result.Errors)
        {
          ModelState.AddModelError(error.Code, error.Description);
        }

        return ValidationProblem(ModelState);
      }

      if (!await roleManager.RoleExistsAsync(Roles.User))
      {
        await roleManager.CreateAsync(new IdentityRole(Roles.User));
      }

      await userManager.AddToRoleAsync(user, Roles.User);

      return Created();
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
      var user = await userManager.FindByEmailAsync(request.Email)
        ?? await userManager.FindByNameAsync(request.Email);

      if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
      {
        return Unauthorized();
      }

      var roles = await userManager.GetRolesAsync(user);
      var token = tokenService.CreateToken(user, roles);

      return new LoginResponse(
        token,
        DateTime.UtcNow.AddMinutes(jwtOptions.Value.ExpiryMinutes),
        new AuthUserResponse(user.Id, user.Email ?? string.Empty, user.FirstName, user.LastName, roles));
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<AuthUserResponse>> Me()
    {
      var user = await userManager.GetUserAsync(User);

      if (user is null)
      {
        return Unauthorized();
      }

      var roles = await userManager.GetRolesAsync(user);

      return new AuthUserResponse(user.Id, user.Email ?? string.Empty, user.FirstName, user.LastName, roles);
    }
  }
}
