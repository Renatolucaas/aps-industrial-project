using IndustrialAPS.Domain.Entities;
using IndustrialAPS.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IndustrialAPS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BOMItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BOMItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/bomitems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BOMItem>>> GetBOMItems()
        {
            return await _context.BOMItems
                .Include(b => b.Product)
                .Include(b => b.Material)
                .ToListAsync();
        }

        // GET: api/bomitems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BOMItem>> GetBOMItem(int id)
        {
            var bomItem = await _context.BOMItems
                .Include(b => b.Product)
                .Include(b => b.Material)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bomItem == null)
            {
                return NotFound();
            }

            return bomItem;
        }

        // POST: api/bomitems
        [HttpPost]
        public async Task<ActionResult<BOMItem>> PostBOMItem(BOMItem bomItem)
        {
            _context.BOMItems.Add(bomItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBOMItem), new { id = bomItem.Id }, bomItem);
        }

        // PUT: api/bomitems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBOMItem(int id, BOMItem bomItem)
        {
            if (id != bomItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(bomItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BOMItemExists(id))
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

        // DELETE: api/bomitems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBOMItem(int id)
        {
            var bomItem = await _context.BOMItems.FindAsync(id);
            if (bomItem == null)
            {
                return NotFound();
            }

            _context.BOMItems.Remove(bomItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BOMItemExists(int id)
        {
            return _context.BOMItems.Any(e => e.Id == id);
        }
    }
}