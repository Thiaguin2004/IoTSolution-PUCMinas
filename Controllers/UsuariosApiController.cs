using IoTSolution.Data;
using IoTSolution.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IoTSolution.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuariosApiController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public UsuariosApiController(ApiDbContext context)
        {
            _context = context;
        }

        // POST api/usuarios
        [HttpPost]
        public async Task<ActionResult<UsuariosModel>> PostUsuario(UsuariosModel usuario)
        {
            if (_context.Usuarios.Any(u => u.CPF == usuario.CPF || u.Telefone == usuario.Telefone))
                return BadRequest("Usuário com o mesmo CPF ou Telefone já existe.");

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.IdUsuario }, usuario);
        }

        // GET api/usuarios/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuariosModel>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        // GET api/usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuariosModel>>> GetAllUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();

            if (usuarios == null || usuarios.Count == 0)
                return NotFound();

            return Ok(usuarios);
        }

        // PUT api/usuarios/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, UsuariosModel usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Usuarios.Any(e => e.IdUsuario == id))
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

    }
}
