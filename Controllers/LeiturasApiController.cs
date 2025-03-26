using IoTSolution.Data;
using IoTSolution.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IoTSolution.Controllers
{
    [ApiController]
    [Route("api/leituras")]
    public partial class LeiturasApiController : Controller
    {
        private readonly ApiDbContext _context;

        public LeiturasApiController(ApiDbContext context)
        {
            _context = context;
        }

        // POST api/strings
        [HttpPost]
        public async Task<ActionResult<LeiturasModel>> PostString(string text, int dispositivo, int sensor)
        {
            LeiturasModel model = new LeiturasModel
            {
                IdDispositivo = dispositivo,
                IdSensor = sensor,
                Temperatura = text,
                DataHoraLeitura = DateTime.Now
            };

            if (model == null || string.IsNullOrEmpty(model.Temperatura))
                return BadRequest("Texto não pode ser nulo ou vazio.");

            if (_context.Dispositivos.Find(dispositivo) == null)
                return BadRequest("Insira um dispositivo que já exista.");

            if (_context.Sensors.Find(sensor) == null)
                return BadRequest("Insira um sensor que já exista.");

            _context.Leituras.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetString), new { id = model.IdLeitura }, model);
        }

        // GET api/strings/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<LeiturasModel>> GetString(int id)
        {
            var stringModel = await _context.Leituras.FindAsync(id);

            if (stringModel == null)
                return NotFound();

            return Ok(stringModel);
        }

        // GET api/strings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeiturasModel>>> GetAll()
        {
            var strings = await _context.Leituras.ToListAsync();

            if (strings == null || strings.Count == 0)
                return NotFound();

            return Ok(strings);
        }
    }
}
