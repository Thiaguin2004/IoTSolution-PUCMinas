using IoTSolution.Data;
using IoTSolution.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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
        // GET api/leituras (com filtros e paginação)
        [HttpGet("paginado")]
        public async Task<ActionResult<IEnumerable<LeiturasModel>>> GetLeituras(
            int? dispositivo,
            int? sensor,
            DateTime? dataInicial,
            DateTime? dataFinal,
            decimal? temperaturaInicial,
            decimal? temperaturaFinal,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var leiturasQuery = _context.Leituras.AsQueryable();

            if (dispositivo.HasValue && dispositivo != 0)
                leiturasQuery = leiturasQuery.Where(l => l.IdDispositivo == dispositivo);

            if (sensor.HasValue && sensor != 0)
                leiturasQuery = leiturasQuery.Where(l => l.IdSensor == sensor);

            if (dataInicial.HasValue)
                leiturasQuery = leiturasQuery.Where(l => l.DataHoraLeitura >= dataInicial.Value);

            if (dataFinal.HasValue)
                leiturasQuery = leiturasQuery.Where(l => l.DataHoraLeitura <= dataFinal.Value);

            if (temperaturaInicial.HasValue)
                leiturasQuery = leiturasQuery.Where(l => l.Temperatura >= temperaturaInicial.Value);

            if (temperaturaFinal.HasValue)
                leiturasQuery = leiturasQuery.Where(l => l.Temperatura <= temperaturaFinal.Value);

            var totalItems = await leiturasQuery.CountAsync();

            var leituras = await leiturasQuery
                .OrderByDescending(l => l.IdLeitura)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = leituras
            });
        }

        // GET api/leituras (com filtros)
        [HttpGet("relatorio")]
        public async Task<ActionResult<IEnumerable<LeiturasModel>>> GetLeiturasRelatorio(
            int? dispositivo,
            int? sensor,
            DateTime? dataInicial,
            DateTime? dataFinal)
        {
            var leiturasQuery = _context.Leituras.AsQueryable();

            if (dispositivo.HasValue && dispositivo != 0)
                leiturasQuery = leiturasQuery.Where(l => l.IdDispositivo == dispositivo);

            if (sensor.HasValue && sensor != 0)
                leiturasQuery = leiturasQuery.Where(l => l.IdSensor == sensor);

            if (dataInicial.HasValue)
                leiturasQuery = leiturasQuery.Where(l => l.DataHoraLeitura >= dataInicial.Value);

            if (dataFinal.HasValue)
                leiturasQuery = leiturasQuery.Where(l => l.DataHoraLeitura <= dataFinal.Value);

            var leituras = await leiturasQuery
                .OrderByDescending(l => l.IdLeitura)
                .ToListAsync();

            if (leituras == null || leituras.Count == 0)
                return Ok(null);

            return Ok(leituras);
        }

        // POST api/leituras
        [HttpPost]
        public async Task<ActionResult<LeiturasModel>> PostString(string text, int dispositivo, int sensor)
        {
            LeiturasModel model = new LeiturasModel
            {
                IdDispositivo = dispositivo,
                IdSensor = sensor,
                Temperatura = Convert.ToDecimal(text, CultureInfo.InvariantCulture),
                DataHoraLeitura = DateTime.Now
            };

            if (_context.Dispositivos.Find(dispositivo) == null)
                return BadRequest("Insira um dispositivo que já exista.");

            if (_context.Sensors.Find(sensor) == null)
                return BadRequest("Insira um sensor que já exista.");

            _context.Leituras.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetString), new { id = model.IdLeitura }, model);
        }

        // POST api/logs
        [HttpPost("logs")]
        public async Task<ActionResult<LogsModel>> PostLog(string log, int dispositivo, int sensor)
        {
            LogsModel model = new LogsModel
            {
                IdDispositivo = dispositivo,
                IdSensor = sensor,
                LogString = log,
                DataHoraLog = DateTime.Now
            };

            if (_context.Dispositivos.Find(dispositivo) == null)
                return BadRequest("Insira um dispositivo que já exista.");

            if (_context.Sensors.Find(sensor) == null)
                return BadRequest("Insira um sensor que já exista.");

            _context.Logs.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetString), new { id = model.IdLog }, model);
        }

        // GET api/leituras/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<LeiturasModel>> GetString(int id)
        {
            var stringModel = await _context.Leituras.FindAsync(id);

            if (stringModel == null)
                return NotFound();

            return Ok(stringModel);
        }

        // GET api/leituras
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<LeiturasModel>>> GetAll()
        //{
        //    var strings = await _context.Leituras.ToListAsync();

        //    if (strings == null || strings.Count == 0)
        //        return NotFound();

        //    return Ok(strings);
        //}
    }
}
