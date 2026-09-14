using IndustrialAPS.Domain.Entities;
using IndustrialAPS.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IndustrialAPS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanningController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlanningController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/planning/run
        [HttpPost("run")]
        public async Task<IActionResult> RunPlanning([FromBody] PlanningRequest request)
        {
            // 1. Validar a requisição
            if (request == null || request.ProductId <= 0 || request.Quantity <= 0)
            {
                return BadRequest("Dados inválidos. Informe ProductId e Quantity.");
            }

            // 2. Buscar o produto
            var product = await _context.Products
                .Include(p => p.BOMItems)
                .Include(p => p.Operations)
                .FirstOrDefaultAsync(p => p.Id == request.ProductId);

            if (product == null)
            {
                return NotFound($"Produto com ID {request.ProductId} não encontrado.");
            }

            // 3. Criar uma ordem de produção
            var order = new ProductionOrder
            {
                ProductId = product.Id,
                Quantity = request.Quantity,
                DueDate = request.DueDate ?? DateTime.Now.AddDays(7),
                Status = OrderStatus.Planned
            };

            _context.ProductionOrders.Add(order);
            await _context.SaveChangesAsync();

            // 4. Simular um agendamento simples (para demonstrar o fluxo)
            // Em um sistema real, aqui entraria o algoritmo de árvore/grafo.
            var machine = await _context.Machines.FirstOrDefaultAsync();
            if (machine != null && product.Operations.Any())
            {
                var operation = product.Operations.First();
                var schedule = new Schedule
                {
                    OrderId = order.Id,
                    OperationId = operation.Id,
                    MachineId = machine.Id,
                    Start = DateTime.Now,
                    End = DateTime.Now.AddMinutes((double)(operation.Duration * request.Quantity))
                };

                _context.Schedules.Add(schedule);
                await _context.SaveChangesAsync();
            }

            // 5. Retornar o resultado
            return Ok(new
            {
                Message = "Planejamento executado com sucesso!",
                OrderId = order.Id,
                Product = product.Name,
                Quantity = order.Quantity,
                Status = order.Status.ToString()
            });
        }

        // GET: api/planning/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlanningResult(int id)
        {
            var order = await _context.ProductionOrders
                .Include(o => o.Product)
                .Include(o => o.Schedules)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound($"Ordem de produção com ID {id} não encontrada.");
            }

            return Ok(new
            {
                OrderId = order.Id,
                Product = order.Product?.Name,
                Quantity = order.Quantity,
                DueDate = order.DueDate,
                Status = order.Status.ToString(),
                Schedules = order.Schedules.Select(s => new
                {
                    s.Id,
                    s.MachineId,
                    s.OperationId,
                    s.Start,
                    s.End
                })
            });
        }
    }

    public class PlanningRequest
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public DateTime? DueDate { get; set; }
    }
}