using IoTSolution.Data;
using IoTSolution.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace IoTSolution.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public UsuariosController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(baseUrl + "usuarios");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var usuarios = JsonConvert.DeserializeObject<List<UsuariosModel>>(jsonString);
                return View(usuarios);
            }

            return View("Error");
        }

        public IActionResult Create()
        {
            return View(new UsuariosModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuariosModel model)
        {
            if (ModelState.IsValid)
            {
                var baseUrl = _configuration["ApiSettings:BaseUrl"];
                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsJsonAsync(baseUrl + "usuarios", model);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View("Error");
                }
            }

            return View(model);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(baseUrl + $"usuarios/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var usuario = JsonConvert.DeserializeObject<UsuariosModel>(jsonString);
                if (usuario == null)
                {
                    return NotFound();
                }

                return View(usuario);
            }

            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsuariosModel model)
        {
            if (id != model.IdUsuario)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var baseUrl = _configuration["ApiSettings:BaseUrl"];
                var client = _httpClientFactory.CreateClient();
                var response = await client.PutAsJsonAsync(baseUrl + $"usuarios/{id}", model);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));  // Redireciona para a lista de usuários
                }
                else
                {
                    return View("Error");
                }
            }

            return View(model);  // Retorna o formulário com erros, se houver
        }
    }
}
