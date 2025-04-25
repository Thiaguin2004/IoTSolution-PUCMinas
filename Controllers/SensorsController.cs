using IoTSolution.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using NuGet.DependencyResolver;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IoTSolution.Controllers
{
    [Authorize]
    public partial class SensorsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public SensorsController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        // Método Index (já existente)
        public async Task<IActionResult> Index()
        {
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(baseUrl + "sensors");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var sensors = JsonConvert.DeserializeObject<List<SensorsModel>>(jsonString);
                return View(sensors);
            }

            return View("Error");
        }

        // Método Create GET
        public async Task<IActionResult> Create()
        {
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            var client = _httpClientFactory.CreateClient();

            // Busca a lista de dispositivos
            var response = await client.GetAsync(baseUrl + "dispositivos");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var dispositivos = JsonConvert.DeserializeObject<List<DispositivosModel>>(json);

                ViewBag.Dispositivos = dispositivos.Select(d => new SelectListItem
                {
                    Value = d.IdDispositivo.ToString(),
                    Text = d.Descricao
                }).ToList();
            }

            return View(new SensorsModel());
        }


        // Método Create POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SensorsModel model)
        {
            if (ModelState.IsValid)
            {
                var baseUrl = _configuration["ApiSettings:BaseUrl"];
                var client = _httpClientFactory.CreateClient();
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync(baseUrl + "sensors", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Erro ao criar o sensor.");
            }

            return View(model);
        }

        // Método Edit GET
        public async Task<IActionResult> Edit(int id)
        {
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            var client = _httpClientFactory.CreateClient();

            var sensorResponse = await client.GetAsync(baseUrl + $"sensors/{id}");
            if (!sensorResponse.IsSuccessStatusCode)
                return View("Error");

            var jsonString = await sensorResponse.Content.ReadAsStringAsync();
            var sensor = JsonConvert.DeserializeObject<SensorsModel>(jsonString);
            if (sensor == null)
                return NotFound();

            var dispositivosResponse = await client.GetAsync(baseUrl + "dispositivos");
            if (dispositivosResponse.IsSuccessStatusCode)
            {
                var dispositivosJson = await dispositivosResponse.Content.ReadAsStringAsync();
                var dispositivos = JsonConvert.DeserializeObject<List<DispositivosModel>>(dispositivosJson);

                ViewBag.Dispositivos = dispositivos.Select(d => new SelectListItem
                {
                    Value = d.IdDispositivo.ToString(),
                    Text = d.Descricao
                }).ToList();
            }

            return View(sensor);
        }


        // Método Edit POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SensorsModel model)
        {
            if (ModelState.IsValid)
            {
                var baseUrl = _configuration["ApiSettings:BaseUrl"];
                var client = _httpClientFactory.CreateClient();
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PutAsync(baseUrl + "sensors/" + id, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Erro ao editar o sensor.");
            }

            return View(model);
        }
    }
}
