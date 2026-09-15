using IndustrialAPS.API.DTOs;
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
        public async Task<ActionResult<IEnumerable<object>>> GetBOMItems()
        {
            return await _context.BOMItems
                .Select(b => new
                {
                    b.Id,
                    b.ProductId,
                    ProductName = b.Product.Name,
                    b.ComponentId,
                    MaterialName = b.Material.Name,
                    b.Quantity
                })
                .ToListAsync();
        }

        // GET: api/bomitems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetBOMItem(int id)
        {
            var bomItem = await _context.BOMItems
                .Where(b => b.Id == id)
                .Select(b => new
                {
                    b.Id,
                    b.ProductId,
                    ProductName = b.Product.Name,
                    b.ComponentId,
                    MaterialName = b.Material.Name,
                    b.Quantity
                })
                .FirstOrDefaultAsync();

            if (bomItem == null)
            {
                return NotFound();
            }

            return bomItem;
        }

        // POST: api/bomitems
        [HttpPost]
        public async Task<IActionResult> PostBOMItem(BOMItemDto dto)
        {
            // Verificar se o produto existe
            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null)
            {
                return BadRequest($"Produto com ID {dto.ProductId} não encontrado.");
            }

            // Verificar se o material existe
            var material = await _context.Materials.FindAsync(dto.ComponentId);
            if (material == null)
            {
                return BadRequest($"Material com ID {dto.ComponentId} não encontrado.");
            }

            var bomItem = new BOMItem
            {
                ProductId = dto.ProductId,
                ComponentId = dto.ComponentId,
                Quantity = dto.Quantity
            };

            _context.BOMItems.Add(bomItem);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "BOMItem criado com sucesso!",
                bomItem.Id,
                bomItem.ProductId,
                bomItem.ComponentId,
                bomItem.Quantity
            });
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