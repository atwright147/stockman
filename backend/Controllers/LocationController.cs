using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockMan.Contracts;
using StockMan.Data;
using StockMan.Models;

namespace StockMan.Controllers;

[Route("api/locations")]
[ApiController]
[Authorize]
public class LocationController(ApplicationDbContext context) : ControllerBase
{
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
  {
    return context.Locations.ToList();
  }

  // GET: api/location/5
  [HttpGet("{id}")]
  public async Task<ActionResult<LocationResponse>> GetLocation(int id)
  {
    var product = await context.Locations
      .Where(l => l.Id == id)
      .Select(l => new LocationResponse(
        l.Id,
        l.Aisle,
        l.Shelf,
        l.Container,
        l.Code,
        l.IsActive,
        l.CreatedAt,
        l.UpdatedAt
      ))
      .FirstOrDefaultAsync();

    return product ?? (ActionResult<LocationResponse>)NotFound();
  }
}
