using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockMan.Contracts;
using StockMan.Data;
using StockMan.Models;

namespace StockMan.Controllers;

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

  // POST: api/products
  [HttpPost()]
  public async Task<ActionResult<ProductResponse>> PostProduct(ProductCreateRequest product)
  {
    var entity = new Product
    {
      Sku = product.Sku,
      Name = product.Name,
      Description = product.Description,
      Barcode = product.Barcode,
      ReorderThreshold = product.ReorderThreshold,
      MinimumReorderQuantity = product.MinimumReorderQuantity,
      UnitCost = product.UnitCost,
      CreatedAt = DateTime.UtcNow,
    };

    context.Products.Add(entity);
    await context.SaveChangesAsync();

    var response = new ProductResponse(
      entity.Id,
      entity.Sku,
      entity.Name,
      entity.Description,
      entity.Barcode,
      entity.ReorderThreshold,
      entity.MinimumReorderQuantity,
      entity.UnitCost,
      entity.CreatedAt,
      entity.UpdatedAt
    );

    return CreatedAtAction(nameof(GetProduct), new { id = entity.Id }, response);
  }

  // PUT: api/products
  [HttpPut("{id}")]
  public async Task<ActionResult<ProductResponse>> PutProduct(int id, ProductUpdateRequest product)
  {
    var existingEntity = await context.Products.FirstOrDefaultAsync(p => p.Id == id);

    if (existingEntity == null)
    {
      return NotFound();
    }

    existingEntity.Sku = product.Sku ?? "";
    existingEntity.Name = product.Name ?? "";
    existingEntity.Description = product.Description;
    existingEntity.Barcode = product.Barcode;
    existingEntity.ReorderThreshold = product.ReorderThreshold ?? 0;
    existingEntity.MinimumReorderQuantity = product.MinimumReorderQuantity ?? 0;
    existingEntity.UnitCost = product.UnitCost ?? 0;
    existingEntity.UpdatedAt = DateTime.UtcNow;

    await context.SaveChangesAsync();

    return StatusCode(StatusCodes.Status201Created, existingEntity);
  }
}

