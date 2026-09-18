using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StockMan.Entities;
using StockMan.Models;

namespace StockMan.Data
{
  public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
  {
    public DbSet<Product> Products { get; set; }
  }
}
