using IoTSolution.Data;
using IoTSolution.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IoTSolution.Controllers
{
    [ApiController]
    [Route("api/dispositivos")]
    public partial class DispositivosApiController : Controller
    {
        private readonly ApiDbContext _context;

        public DispositivosApiController(ApiDbContext context)
        {
            _context = context;
        }

        // POST api/dispositivos
        [HttpPost]
        public async Task<ActionResult<DispositivosModel>> PostString(string text)
        {
            DispositivosModel model = new DispositivosModel
            {
                Descricao = text,
                DataHoraCadastro = DateTime.Now
            };

            if (model == null || string.IsNullOrEmpty(model.Descricao))
                return BadRequest("Texto não pode ser nulo ou vazio.");

            _context.Dispositivos.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetString), new { id = model.IdDispositivo }, model);
        }

        // GET api/dispositivos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DispositivosModel>> GetString(int id)
        {
            var model = await _context.Dispositivos.FindAsync(id);

            if (model == null)
                return NotFound();

            return Ok(model);
        }

        // GET api/dispositivos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DispositivosModel>>> GetAll()
        {
            var dispositivos = await _context.Dispositivos.ToListAsync();

            if (dispositivos == null || dispositivos.Count == 0)
                return NotFound();

            return Ok(dispositivos);
        }
    }
}
