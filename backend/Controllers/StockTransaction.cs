using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockMan.Contracts;
using StockMan.Data;
using StockMan.Models;

namespace StockMan.Controllers
{
  [Route("api/stock-transactions")]
  [ApiController]
  [Authorize]
  public class StockTransactionController(ApplicationDbContext context) : ControllerBase
  {
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StockTransaction>>> GetStockTransactions()
    {
      return context.StockTransaction.ToList();
    }

    // GET: api/stock-transaction/5
    [HttpGet("{id}")]
    public async Task<ActionResult<StockTransactionResponse>> GetStockTransaction(int id)
    {
      var product = await context.StockTransaction
        .Where(st => st.Id == id)
        .Select(st => new StockTransactionResponse(
          st.Id,
          st.ProductId,
          st.Product,
          st.FromLocationId,
          st.FromLocation,
          st.ToLocationId,
          st.ToLocation,
          st.Quantity,
          st.Type,
          st.Reason,
          st.TimestampUtc,
          st.CreatedByUserId
        ))
        .FirstOrDefaultAsync();

      return product ?? (ActionResult<StockTransactionResponse>)NotFound();
    }
  }
}
