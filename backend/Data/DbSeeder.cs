using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StockMan.Entities;
using StockMan.Models;

namespace StockMan.Data
{
  public class DbSeeder(
    ApplicationDbContext db,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    int productCount = 100,
    int locationCount = 25,
    int stockLevelCount = 25
  )
  {
    public async Task SeedAsync()
    {
      await db.Database.MigrateAsync();

      await SeedRolesAndAdminAsync();

      if (await db.Products.AnyAsync())
      {
        return;
      }

      var fakerProducts = new Faker<Product>()
        .RuleFor(p => p.Sku, f => $"SKU-{f.Commerce.Ean8()}-{f.IndexGlobal}")
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Barcode, f => f.Commerce.Ean13())
        .RuleFor(p => p.ReorderThreshold, f => f.Random.Int(1, 50))
        .RuleFor(p => p.MinimumReorderQuantity, (f, p) => f.Random.Int(p.ReorderThreshold, 100))
        .RuleFor(p => p.UnitCost, f => decimal.Parse(f.Commerce.Price(0.5m, 500m)))
        .RuleFor(p => p.CreatedAt, f => f.Date.Past(1))
        .RuleFor(p => p.UpdatedAt, (f, p) => f.Random.Bool(0.5f) ? f.Date.Between(p.CreatedAt, DateTime.UtcNow) : null);

      var products = fakerProducts.Generate(productCount);

      await db.Products.AddRangeAsync(products);

      var fakerLocations = new Faker<Location>()
        .RuleFor(l => l.Aisle, f => f.Commerce.ProductName())
        .RuleFor(l => l.Shelf, f => f.Commerce.ProductName())
        .RuleFor(l => l.Container, f => f.Commerce.ProductDescription())
        .RuleFor(l => l.Code, f => f.Commerce.Ean13())
        .RuleFor(l => l.IsActive, f => f.Random.Bool());

      var locations = fakerLocations.Generate(locationCount);

      await db.Locations.AddRangeAsync(locations);

      // Persist products and locations first so their database-generated Ids
      // are available for the stock level foreign keys below.
      await db.SaveChangesAsync();

      // StockLevels has a unique index on (ProductId, LocationId), so build
      // distinct pairs from the cartesian product of the persisted Ids.
      var distinctPairs = (from product in products
                           from location in locations
                           select (ProductId: product.Id, LocationId: location.Id))
        .OrderBy(_ => Guid.NewGuid())
        .Take(stockLevelCount)
        .ToList();

      var fakerStockLevels = new Faker<StockLevel>()
        .RuleFor(sl => sl.QuantityOnHand, f => f.Random.Int(0, 500))
        .RuleFor(sl => sl.QuantityAllocated, (f, sl) => f.Random.Int(0, sl.QuantityOnHand));

      var stockLevels = fakerStockLevels.Generate(distinctPairs.Count);

      for (var i = 0; i < stockLevels.Count; i++)
      {
        stockLevels[i].ProductId = distinctPairs[i].ProductId;
        stockLevels[i].LocationId = distinctPairs[i].LocationId;
      }

      await db.StockLevel.AddRangeAsync(stockLevels);

      await db.SaveChangesAsync();
    }

    private async Task SeedRolesAndAdminAsync()
    {
      if (!await roleManager.RoleExistsAsync(Roles.Admin))
      {
        await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
      }

      if (!await roleManager.RoleExistsAsync(Roles.User))
      {
        await roleManager.CreateAsync(new IdentityRole(Roles.User));
      }

      const string adminEmail = "admin@stockman.local";

      if (await userManager.FindByEmailAsync(adminEmail) is not null)
      {
        return;
      }

      var admin = new ApplicationUser
      {
        UserName = adminEmail,
        Email = adminEmail,
        FirstName = "Admin",
        LastName = "User",
        EmailConfirmed = true,
      };

      var result = await userManager.CreateAsync(admin, "P@ssword1");

      if (result.Succeeded)
      {
        await userManager.AddToRoleAsync(admin, Roles.Admin);
      }
    }
  }
}
