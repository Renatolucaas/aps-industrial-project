using IndustrialAPS.Domain.Algorithms.Graph;
using IndustrialAPS.Domain.Entities;
using IndustrialAPS.Domain.Services;
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
        private readonly BOMService _bomService;
        private readonly GraphService _graphService;

        public PlanningController(ApplicationDbContext context, BOMService bomService, GraphService graphService)
        {
            _context = context;
            _bomService = bomService;
            _graphService = graphService;
        }

        // POST: api/planning/run
        [HttpPost("run")]
        public async Task<IActionResult> RunPlanning([FromBody] PlanningRequest request)
        {
            if (request == null || request.ProductId <= 0 || request.Quantity <= 0)
            {
                return BadRequest("Dados inválidos. Informe ProductId e Quantity.");
            }

            // 1. Buscar o produto
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == request.ProductId);

            if (product == null)
            {
                return NotFound($"Produto com ID {request.ProductId} não encontrado.");
            }

            // 2. Buscar os dados necessários para a árvore
            var allBomItems = await _context.BOMItems.ToListAsync();
            var allMaterials = await _context.Materials.ToListAsync();

            // 3. Montar a árvore BOM
            var bomTree = _bomService.BuildBOMTree(product, allBomItems, allMaterials);

            // 4. Calcular a necessidade de materiais
            var materialRequirements = _bomService.CalculateMaterialRequirements(bomTree, request.Quantity);

            // 5. Buscar máquinas e conexões para o grafo
            var machines = await _context.Machines.ToListAsync();
            var connections = await _context.MachineConnections.ToListAsync();

            // 6. Montar o grafo
            var graph = _graphService.BuildGraph(machines, connections);

            // 7. Criar a ordem de produção
            var order = new ProductionOrder
            {
                ProductId = product.Id,
                Quantity = request.Quantity,
                DueDate = request.DueDate ?? DateTime.Now.AddDays(7),
                Status = OrderStatus.Planned
            };

            _context.ProductionOrders.Add(order);
            await _context.SaveChangesAsync();

            // 8. Retornar o resultado completo
            return Ok(new
            {
                Message = "Planejamento executado com sucesso!",
                OrderId = order.Id,
                Product = product.Name,
                Quantity = order.Quantity,
                DueDate = order.DueDate,
                Status = order.Status.ToString(),
                MaterialsRequired = materialRequirements,
                FactoryGraph = new
                {
                    TotalMachines = graph.Vertices.Count,
                    TotalConnections = graph.Vertices.Sum(v => v.Edges.Count)
                }
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