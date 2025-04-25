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
        public async Task<ActionResult<DispositivosModel>> PostString(DispositivosModel model)
        {
            if (model == null)
                return BadRequest("Modelo de sensor não pode ser nulo.");

            // Verifica se o dispositivo existe
            if (_context.Dispositivos.Find(model.IdUsuario) == null)
                return BadRequest("Usuário não encontrado.");

            if (model == null || string.IsNullOrEmpty(model.Descricao))
                return BadRequest("Texto não pode ser nulo ou vazio.");

            model.DataHoraCadastro = DateTime.Now;

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

        // PUT api/dispositivos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDispositivo(int id, DispositivosModel model)
        {
            // Verifica se o modelo enviado é nulo
            if (model == null)
            {
                return BadRequest("Modelo de dispositivo não pode ser nulo.");
            }

            // Verifica se o dispositivo existe
            var existingDispositivo = await _context.Dispositivos.FindAsync(id);
            if (existingDispositivo == null)
            {
                return NotFound("Dispositivo não encontrado.");
            }

            // Verifica se o usuário existe
            var usuarioExistente = await _context.Usuarios.FindAsync(model.IdUsuario);
            if (usuarioExistente == null)
            {
                return BadRequest("Usuário não encontrado.");
            }

            // Atualiza os campos do dispositivo
            existingDispositivo.Descricao = model.Descricao;
            existingDispositivo.IdUsuario = model.IdUsuario;
            existingDispositivo.DataHoraCadastro = DateTime.Now; // Atualiza a data de cadastro ou mantém a existente, se necessário

            // Salva as alterações no banco de dados
            _context.Dispositivos.Update(existingDispositivo);
            await _context.SaveChangesAsync();

            return NoContent(); // Retorna 204 No Content quando a atualização for bem-sucedida
        }
    }
}
