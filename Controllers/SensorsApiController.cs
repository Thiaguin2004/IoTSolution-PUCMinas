using IoTSolution.Data;
using IoTSolution.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace IoTSolution.Controllers
{
    [ApiController]
    [Route("api/sensors")]
    public partial class SensorsApiController : Controller
    {
        private readonly ApiDbContext _context;

        public SensorsApiController(ApiDbContext context)
        {
            _context = context;
        }

        // POST api/sensors
        [HttpPost]
        public async Task<ActionResult<SensorsModel>> PostString(string descricao, int dispositivo)
        {
            SensorsModel model = new SensorsModel
            {
                Descricao = descricao,
                DataHoraCadastro = DateTime.Now,
                IdDispositivo = dispositivo
            };

            if(_context.Dispositivos.Find(dispositivo) == null)
                return BadRequest("Insira um dispositivo que já exista.");

            if (model == null)
                return BadRequest("Texto não pode ser nulo ou vazio.");

            _context.Sensors.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetString), new { id = model.IdSensor }, model);
        }

        // GET api/sensors/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SensorsModel>> GetString(int id)
        {
            var stringModel = await _context.Sensors.FindAsync(id);

            if (stringModel == null)
                return NotFound();

            return Ok(stringModel);
        }

        // GET api/sensors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SensorsModel>>> GetAll()
        {
            var sensors = await _context.Sensors.ToListAsync();

            if (sensors == null || sensors.Count == 0)
                return NotFound();

            return Ok(sensors);
        }
    }
}
