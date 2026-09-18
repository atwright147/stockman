using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StockMan.Data;
using StockMan.Entities;
using StockMan.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<DbSeeder>();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>() ?? new JwtSettings();

if (string.IsNullOrEmpty(jwtSettings.Key) || jwtSettings.Key.Length < 32)
{
  throw new InvalidOperationException(
    "Jwt:Key is missing or too short (must be at least 32 characters). " +
    "Set it via: dotnet user-secrets set \"Jwt:Key\" \"<a-strong-random-key-at-least-32-chars>\"");
}

builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);
builder.Services.AddSingleton<TokenService>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
  options.Password.RequireNonAlphanumeric = false;
  options.Password.RequireDigit = true;
  options.Password.RequiredLength = 8;
  options.Password.RequireUppercase = true;
  options.Password.RequireLowercase = true;
})
  .AddEntityFrameworkStores<ApplicationDbContext>()
  .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
  .AddJwtBearer(options =>
  {
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      ClockSkew = TimeSpan.Zero,
      ValidIssuer = jwtSettings.Issuer,
      ValidAudience = jwtSettings.Audience,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
    };
  });

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
  var dbSeeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
  await dbSeeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
