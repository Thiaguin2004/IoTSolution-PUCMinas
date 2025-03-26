using IoTSolution.Data;
using IoTSolution.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IoTSolution.Controllers
{
    [ApiController]
    [Route("api/usuarios/")]
    public class AccountApiController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public AccountApiController(ApiDbContext context)
        {
            _context = context;
        }

        // GET api/usuarios/{usuario}/{senha}
        [HttpGet("{usuario}/{senha}")]
        public async Task<ActionResult<UsuariosModel>> GetUsuario(string usuario, string senha)
        {
            var model = await _context.Usuarios.FirstOrDefaultAsync(u => u.Usuario == usuario && u.Senha == senha);

            if (model == null)
                return NotFound();

            return Ok(model);
        }
    }
}
