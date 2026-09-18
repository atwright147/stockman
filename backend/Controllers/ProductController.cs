using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockMan.Contracts;
using StockMan.Data;
using StockMan.Models;

namespace StockMan.Controllers
{
  [Route("api/products")]
  [ApiController]
  [Authorize]
  public class ProductController(ApplicationDbContext context) : ControllerBase
  {
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
      return context.Products.ToList();
    }

    // GET: api/products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponse>> GetProduct(int id)
    {
      var product = await context.Products
        .Where(p => p.Id == id)
        .Select(p => new ProductResponse(
          p.Id, p.Sku, p.Name, p.Description, p.Barcode,
          p.ReorderThreshold, p.MinimumReorderQuantity, p.UnitCost,
          p.CreatedAt, p.UpdatedAt))
        .FirstOrDefaultAsync();

      return product ?? (ActionResult<ProductResponse>)NotFound();
    }
  }
}
