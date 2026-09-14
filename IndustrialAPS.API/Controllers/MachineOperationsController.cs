using IndustrialAPS.API.DTOs;
using IndustrialAPS.Domain.Entities;
using IndustrialAPS.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IndustrialAPS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineOperationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MachineOperationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/machineoperations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetMachineOperations()
        {
            return await _context.MachineOperations
                .Select(mo => new
                {
                    mo.MachineId,
                    mo.OperationId,
                    MachineName = mo.Machine.Name,
                    OperationName = mo.Operation.Name
                })
                .ToListAsync();
        }

        // GET: api/machineoperations/5/3
        [HttpGet("{machineId}/{operationId}")]
        public async Task<ActionResult<object>> GetMachineOperation(int machineId, int operationId)
        {
            var machineOperation = await _context.MachineOperations
                .Where(mo => mo.MachineId == machineId && mo.OperationId == operationId)
                .Select(mo => new
                {
                    mo.MachineId,
                    mo.OperationId,
                    MachineName = mo.Machine.Name,
                    OperationName = mo.Operation.Name
                })
                .FirstOrDefaultAsync();

            if (machineOperation == null)
            {
                return NotFound();
            }

            return machineOperation;
        }

        // POST: api/machineoperations
        [HttpPost]
        public async Task<IActionResult> PostMachineOperation(MachineOperationDto dto)
        {
            // Verificar se a máquina existe
            var machine = await _context.Machines.FindAsync(dto.MachineId);
            if (machine == null)
            {
                return BadRequest($"Máquina com ID {dto.MachineId} não encontrada.");
            }

            // Verificar se a operação existe
            var operation = await _context.Operations.FindAsync(dto.OperationId);
            if (operation == null)
            {
                return BadRequest($"Operação com ID {dto.OperationId} não encontrada.");
            }

            // Verificar se já existe essa associação
            var exists = await _context.MachineOperations
                .AnyAsync(mo => mo.MachineId == dto.MachineId && mo.OperationId == dto.OperationId);

            if (exists)
            {
                return BadRequest("Esta associação já existe.");
            }

            var machineOperation = new MachineOperation
            {
                MachineId = dto.MachineId,
                OperationId = dto.OperationId
            };

            _context.MachineOperations.Add(machineOperation);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Associação criada com sucesso!",
                dto.MachineId,
                dto.OperationId
            });
        }

        // DELETE: api/machineoperations/5/3
        [HttpDelete("{machineId}/{operationId}")]
        public async Task<IActionResult> DeleteMachineOperation(int machineId, int operationId)
        {
            var machineOperation = await _context.MachineOperations
                .FirstOrDefaultAsync(mo => mo.MachineId == machineId && mo.OperationId == operationId);

            if (machineOperation == null)
            {
                return NotFound();
            }

            _context.MachineOperations.Remove(machineOperation);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}