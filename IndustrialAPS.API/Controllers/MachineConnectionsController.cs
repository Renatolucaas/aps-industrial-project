using IndustrialAPS.Domain.Entities;
using IndustrialAPS.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IndustrialAPS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineConnectionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MachineConnectionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/machineconnections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MachineConnection>>> GetMachineConnections()
        {
            return await _context.MachineConnections
                .Include(mc => mc.FromMachine)
                .Include(mc => mc.ToMachine)
                .ToListAsync();
        }

        // GET: api/machineconnections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MachineConnection>> GetMachineConnection(int id)
        {
            var connection = await _context.MachineConnections
                .Include(mc => mc.FromMachine)
                .Include(mc => mc.ToMachine)
                .FirstOrDefaultAsync(mc => mc.Id == id);

            if (connection == null)
            {
                return NotFound();
            }

            return connection;
        }

        // POST: api/machineconnections
        [HttpPost]
        public async Task<ActionResult<MachineConnection>> PostMachineConnection(MachineConnection connection)
        {
            _context.MachineConnections.Add(connection);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMachineConnection), new { id = connection.Id }, connection);
        }

        // PUT: api/machineconnections/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMachineConnection(int id, MachineConnection connection)
        {
            if (id != connection.Id)
            {
                return BadRequest();
            }

            _context.Entry(connection).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MachineConnectionExists(id))
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

        // DELETE: api/machineconnections/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMachineConnection(int id)
        {
            var connection = await _context.MachineConnections.FindAsync(id);
            if (connection == null)
            {
                return NotFound();
            }

            _context.MachineConnections.Remove(connection);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MachineConnectionExists(int id)
        {
            return _context.MachineConnections.Any(e => e.Id == id);
        }
    }
}