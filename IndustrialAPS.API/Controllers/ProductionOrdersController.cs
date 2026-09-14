using IndustrialAPS.Domain.Entities;
using IndustrialAPS.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IndustrialAPS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionOrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductionOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/productionorders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductionOrder>>> GetProductionOrders()
        {
            return await _context.ProductionOrders
                .Include(o => o.Product)
                .ToListAsync();
        }

        // GET: api/productionorders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductionOrder>> GetProductionOrder(int id)
        {
            var order = await _context.ProductionOrders
                .Include(o => o.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // POST: api/productionorders
        [HttpPost]
        public async Task<ActionResult<ProductionOrder>> PostProductionOrder(ProductionOrder order)
        {
            _context.ProductionOrders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductionOrder), new { id = order.Id }, order);
        }

        // PUT: api/productionorders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductionOrder(int id, ProductionOrder order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductionOrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/productionorders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductionOrder(int id)
        {
            var order = await _context.ProductionOrders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.ProductionOrders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductionOrderExists(int id)
        {
            return _context.ProductionOrders.Any(e => e.Id == id);
        }
    }
}