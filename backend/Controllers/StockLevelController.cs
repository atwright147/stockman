using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockMan.Contracts;
using StockMan.Data;
using StockMan.Models;

namespace StockMan.Controllers;

[Route("api/stock-levels")]
[ApiController]
[Authorize]
public class StockLevelController(ApplicationDbContext context) : ControllerBase
{
  [HttpGet]
  public async Task<ActionResult<IEnumerable<StockLevel>>> GetStockLevels()
  {
    return context.StockLevel.ToList();
  }

  // GET: api/location/5
  [HttpGet("{id}")]
  public async Task<ActionResult<StockLevelResponse>> GetStockLevel(int id)
  {
    var product = await context.StockLevel
      .Where(sl => sl.Id == id)
      .Select(sl => new StockLevelResponse(
        sl.Id,
        sl.ProductId,
        sl.LocationId,
        sl.QuantityOnHand,
        sl.QuantityAllocated
      ))
      .FirstOrDefaultAsync();

    return product ?? (ActionResult<StockLevelResponse>)NotFound();
  }
}



