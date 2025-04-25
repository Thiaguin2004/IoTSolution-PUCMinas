using IoTSolution.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using NuGet.DependencyResolver;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IoTSolution.Controllers
{
    [Authorize]
    public partial class DispositivosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public DispositivosController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(baseUrl + "dispositivos");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var dispositivos = JsonConvert.DeserializeObject<List<DispositivosModel>>(jsonString);

                return View(dispositivos);
            }

            return View("Error");
        }

        // Método Create GET
        public async Task<IActionResult> Create()
        {
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync(baseUrl + "usuarios");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var usuarios = JsonConvert.DeserializeObject<List<UsuariosModel>>(jsonString);

                ViewBag.Usuarios = usuarios
                    .Select(u => new SelectListItem
                    {
                        Value = u.IdUsuario.ToString(),
                        Text = u.Nome
                    }).ToList();
            }
            return View(new DispositivosModel());
        }

        // Método Create POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DispositivosModel model)
        {
            if (ModelState.IsValid)
            {
                var baseUrl = _configuration["ApiSettings:BaseUrl"];
                var client = _httpClientFactory.CreateClient();
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync(baseUrl + "dispositivos", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Erro ao criar o dispositivo.");
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync(baseUrl + $"dispositivos/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var dispositivo = JsonConvert.DeserializeObject<DispositivosModel>(jsonString);
            if (dispositivo == null)
            {
                return NotFound();
            }

            var usuariosResponse = await client.GetAsync(baseUrl + "usuarios");
            if (usuariosResponse.IsSuccessStatusCode)
            {
                var usuariosJson = await usuariosResponse.Content.ReadAsStringAsync();
                var usuarios = JsonConvert.DeserializeObject<List<UsuariosModel>>(usuariosJson);

                ViewBag.Usuarios = usuarios.Select(u => new SelectListItem
                {
                    Value = u.IdUsuario.ToString(),
                    Text = u.Nome
                }).ToList();
            }

            return View(dispositivo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DispositivosModel model)
        {
            if (id != model.IdDispositivo)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var baseUrl = _configuration["ApiSettings:BaseUrl"];
                var client = _httpClientFactory.CreateClient();
                var response = await client.PutAsJsonAsync(baseUrl + $"dispositivos/{id}", model);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));  // Redireciona para a lista de dispositivos
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
