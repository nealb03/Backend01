using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyWebApi.Models;    // Cloud495Context / Account / Transaction models
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly Cloud495Context _context;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(Cloud495Context context, ILogger<TransactionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/transactions?userId=1&search=keyword
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? userId, [FromQuery] string? search)
        {
            try
            {
                // ---------------------------------------------------------
                // Base query with navigation to Account
                // ---------------------------------------------------------
                var query = _context.Transactions
                                    .Include(t => t.Account)
                                    .AsQueryable();

                // ---------------------------------------------------------
                // Filter by the user's accounts
                // ---------------------------------------------------------
                if (userId.HasValue)
                {
                    query = query.Where(t => t.Account != null &&
                                             t.Account.UserId == userId.Value);
                }

                // ---------------------------------------------------------
                // Optional keyword search
                // ---------------------------------------------------------
                if (!string.IsNullOrWhiteSpace(search))
                {
                    var lowered = search.ToLower();
                    query = query.Where(t =>
                        (t.Description ?? string.Empty).ToLower().Contains(lowered) ||
                        (t.Type ?? string.Empty).ToLower().Contains(lowered));
                }

                // ---------------------------------------------------------
                // Order newest first, limit 25
                // ---------------------------------------------------------
                var transactions = await query
                    .OrderByDescending(t => t.CreatedAt)
                    .Take(25)
                    .Select(t => new
                    {
                        t.TransactionId,
                        t.Amount,
                        t.Type,
                        t.Description,
                        t.CreatedAt,
                        Account = t.Account == null ? null : new
                        {
                            t.Account.AccountId,
                            t.Account.AccountNumber,
                            t.Account.AccountType,
                            t.Account.UserId
                        }
                    })
                    .ToListAsync();

                return Ok(transactions);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching transactions");
                return StatusCode(500, new { message = "Internal Server Error" });
            }
        }
    }
}