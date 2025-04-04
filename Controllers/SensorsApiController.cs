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
        public async Task<ActionResult<SensorsModel>> PostString(SensorsModel model)
        {
            if (model == null)
                return BadRequest("Modelo de sensor não pode ser nulo.");

            // Verifica se o dispositivo existe
            if (_context.Dispositivos.Find(model.IdDispositivo) == null)
                return BadRequest("Dispositivo não encontrado.");

            // Define a data e hora de cadastro
            model.DataHoraCadastro = DateTime.Now;

            // Adiciona o novo sensor no banco de dados
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

        // PUT api/sensors/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutString(int id, SensorsModel model)
        {
            // Verifica se o modelo enviado é nulo
            if (model == null)
                return BadRequest("Modelo de sensor não pode ser nulo.");

            // Verifica se o sensor existe
            var existingSensor = await _context.Sensors.FindAsync(id);
            if (existingSensor == null)
                return NotFound("Sensor não encontrado.");

            // Verifica se o dispositivo existe
            if (_context.Dispositivos.Find(model.IdDispositivo) == null)
                return BadRequest("Dispositivo não encontrado.");

            // Atualiza os campos do sensor
            existingSensor.Descricao = model.Descricao;
            existingSensor.IdDispositivo = model.IdDispositivo;
            existingSensor.DataHoraCadastro = DateTime.Now; // Caso queira manter a data de criação ou definir uma nova

            // Salva as alterações
            _context.Sensors.Update(existingSensor);
            await _context.SaveChangesAsync();

            return NoContent(); // Retorna 204 No Content quando a atualização for bem-sucedida
        }
    }
}
