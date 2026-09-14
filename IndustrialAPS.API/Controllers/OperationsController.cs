using IndustrialAPS.API.DTOs;
using IndustrialAPS.Domain.Entities;
using IndustrialAPS.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IndustrialAPS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OperationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/operations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OperationResponseDto>>> GetOperations()
        {
            return await _context.Operations
                .Select(o => new OperationResponseDto
                {
                    Id = o.Id,
                    ProductId = o.ProductId,
                    Name = o.Name,
                    Duration = o.Duration
                })
                .ToListAsync();
        }

        // GET: api/operations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OperationResponseDto>> GetOperation(int id)
        {
            var operation = await _context.Operations
                .Where(o => o.Id == id)
                .Select(o => new OperationResponseDto
                {
                    Id = o.Id,
                    ProductId = o.ProductId,
                    Name = o.Name,
                    Duration = o.Duration
                })
                .FirstOrDefaultAsync();

            if (operation == null)
            {
                return NotFound();
            }

            return operation;
        }

        // POST: api/operations
        [HttpPost]
        public async Task<ActionResult<OperationResponseDto>> PostOperation(OperationDto operationDto)
        {
            var product = await _context.Products.FindAsync(operationDto.ProductId);
            if (product == null)
            {
                return BadRequest($"Produto com ID {operationDto.ProductId} não encontrado.");
            }

            var operation = new Operation
            {
                ProductId = operationDto.ProductId,
                Name = operationDto.Name,
                Duration = operationDto.Duration
            };

            _context.Operations.Add(operation);
            await _context.SaveChangesAsync();

            var response = new OperationResponseDto
            {
                Id = operation.Id,
                ProductId = operation.ProductId,
                Name = operation.Name,
                Duration = operation.Duration
            };

            return CreatedAtAction(nameof(GetOperation), new { id = operation.Id }, response);
        }

        // PUT: api/operations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOperation(int id, OperationDto operationDto)
        {
            var operation = await _context.Operations.FindAsync(id);
            if (operation == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(operationDto.ProductId);
            if (product == null)
            {
                return BadRequest($"Produto com ID {operationDto.ProductId} não encontrado.");
            }

            operation.ProductId = operationDto.ProductId;
            operation.Name = operationDto.Name;
            operation.Duration = operationDto.Duration;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OperationExists(id))
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

        // DELETE: api/operations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOperation(int id)
        {
            var operation = await _context.Operations.FindAsync(id);
            if (operation == null)
            {
                return NotFound();
            }

            _context.Operations.Remove(operation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OperationExists(int id)
        {
            return _context.Operations.Any(e => e.Id == id);
        }
    }
}