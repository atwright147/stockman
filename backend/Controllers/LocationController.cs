using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockMan.Contracts;
using StockMan.Data;
using StockMan.Models;
using StockMan.Utils;

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

  // POST api/location/1
  [HttpPost()]
  public async Task<ActionResult<LocationResponse>> PostLocation(LocationCreateRequest location)
  {
    var entity = new Location
    {
      Aisle = location.Aisle,
      Shelf = location.Shelf,
      Container = location.Container,
      Code = location.Code,
      IsActive = location.IsActive,
    };

    context.Locations.Add(entity);
    await context.SaveChangesAsync();

    var response = new LocationResponse(
      entity.Id,
      entity.Aisle,
      entity.Shelf,
      entity.Container,
      entity.Code,
      entity.IsActive,
      entity.CreatedAt,
      entity.UpdatedAt
    );

    return CreatedAtAction(nameof(PostLocation), new { id = entity.Id }, response);
  }

  // PUT: api/locations/2
  [HttpPut("{id}")]
  public async Task<ActionResult<LocationResponse>> PutLocation(int id, LocationUpdateRequest location)
  {
    var existingEntity = await context.Locations.FirstOrDefaultAsync(entity => entity.Id == id);

    if (existingEntity == null)
    {
      return NotFound();
    }

    // this should probably be handled with validation,
    // that is a job for future me (f*@k that guy)
    existingEntity.Aisle = AppUtils.Coalesce(location.Aisle, existingEntity.Aisle);
    existingEntity.Shelf = AppUtils.Coalesce(location.Shelf, existingEntity.Shelf);
    existingEntity.Container = AppUtils.Coalesce(location.Container, existingEntity.Container);
    existingEntity.Code = AppUtils.Coalesce(location.Code, existingEntity.Code);
    existingEntity.IsActive = AppUtils.ToBool(location.IsActive, existingEntity.IsActive);

    await context.SaveChangesAsync();

    return StatusCode(StatusCodes.Status201Created, existingEntity);
  }
}
