using IoTSolution.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using NuGet.DependencyResolver;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IoTSolution.Controllers
{
    [Authorize]
    public partial class LeiturasController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public LeiturasController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetSensoresPorDispositivo(int dispositivoId)
        {
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync($"{baseUrl}sensors");

            if (response.IsSuccessStatusCode)
            {
                var todosSensores = await client.GetFromJsonAsync<List<SensorsModel>>($"{baseUrl}sensors");
                List<SensorsModel> sensoresFiltrados = new List<SensorsModel>();
                if(dispositivoId != 0)
                    sensoresFiltrados = todosSensores.Where(s => s.IdDispositivo == dispositivoId).ToList();
                else
                    sensoresFiltrados = todosSensores.ToList();
                return Json(sensoresFiltrados);
            }

            return BadRequest();
        }

        public async Task<IActionResult> Index(
            int? dispositivo, int? sensor,
            DateTime? dataInicial, DateTime? dataFinal,
            string? temperaturaInicial, string? temperaturaFinal,
            int pageNumber = 1, int pageSize = 10)
        {
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            var client = _httpClientFactory.CreateClient();

            decimal? temperaturaInicialDecimal = null;
            if (!string.IsNullOrEmpty(temperaturaInicial))
            {
                temperaturaInicial = temperaturaInicial.Replace(',', '.');
                if (decimal.TryParse(temperaturaInicial, NumberStyles.Any, new CultureInfo("en-US"), out var temp1))
                {
                    temperaturaInicialDecimal = temp1;
                }
            }

            decimal? temperaturaFinalDecimal = null;
            if (!string.IsNullOrEmpty(temperaturaFinal))
            {
                temperaturaFinal = temperaturaFinal.Replace(',', '.');
                if (decimal.TryParse(temperaturaFinal, NumberStyles.Any, new CultureInfo("en-US"), out var temp2))
                {
                    temperaturaFinalDecimal = temp2;
                }
            }

            var url = $"{baseUrl}leituras/paginado?" +
                $"pageNumber={pageNumber}&pageSize={pageSize}&";

            if (dispositivo.HasValue && dispositivo != 0)
                url += $"dispositivo={dispositivo}&";
            if (sensor.HasValue && sensor != 0)
                url += $"sensor={sensor}&";
            if (dataInicial.HasValue)
                url += $"dataInicial={dataInicial.Value:yyyy-MM-dd}&";
            else
                url += $"dataInicial={DateTime.Now.AddDays(-7):yyyy-MM-dd}&";
            if (dataFinal.HasValue)
                url += $"dataFinal={dataFinal.Value:yyyy-MM-dd}&";
            else
                url += $"dataFinal={DateTime.Now:yyyy-MM-dd}&";
            if (temperaturaInicialDecimal.HasValue)
                url += $"temperaturaInicial={temperaturaInicialDecimal}&";
            if (temperaturaFinalDecimal.HasValue)
                url += $"temperaturaFinal={temperaturaFinalDecimal}&";

            url = url.TrimEnd('&');

            var dispositivosResponse = await client.GetAsync(baseUrl + "dispositivos");
            if (dispositivosResponse.IsSuccessStatusCode)
            {
                var json = await dispositivosResponse.Content.ReadAsStringAsync();
                var dispositivos = JsonConvert.DeserializeObject<List<DispositivosModel>>(json);
                ViewBag.Dispositivos = dispositivos.Select(d => new SelectListItem
                {
                    Value = d.IdDispositivo.ToString(),
                    Text = d.Descricao,
                    Selected = dispositivo.HasValue && d.IdDispositivo == dispositivo.Value
                }).ToList();
            }

            var sensoresResponse = await client.GetAsync(baseUrl + "sensors");
            if (sensoresResponse.IsSuccessStatusCode)
            {
                var json = await sensoresResponse.Content.ReadAsStringAsync();
                var sensores = JsonConvert.DeserializeObject<List<SensorsModel>>(json);
                ViewBag.Sensores = sensores.Select(s => new SelectListItem
                {
                    Value = s.IdSensor.ToString(),
                    Text = s.Descricao,
                    Selected = sensor.HasValue && s.IdSensor == sensor.Value
                }).ToList();
            }

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var pagedResult = JsonConvert.DeserializeObject<PagedLeiturasViewModel>(jsonString);

                ViewBag.PageNumber = pagedResult.PageNumber;
                ViewBag.TotalPages = (int)Math.Ceiling((double)pagedResult.TotalItems / pagedResult.PageSize);

                return View(pagedResult.Items);
            }

            return View("Error");
        }


        public async Task<IActionResult> RelatorioTemperaturas(int? dispositivo, int? sensor, DateTime? dataInicial, DateTime? dataFinal)
        {
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            var client = _httpClientFactory.CreateClient();

            // Adiciona os filtros na URL
            var url = $"{baseUrl}leituras/relatorio?";
            if (dispositivo.HasValue) url += $"dispositivo={dispositivo}&";
            if (sensor.HasValue) url += $"sensor={sensor}&";
            if (dataInicial.HasValue)
                url += $"dataInicial={dataInicial.Value:yyyy-MM-dd}&";
            else
                url += $"dataInicial={DateTime.Now.AddDays(-7):yyyy-MM-dd}&";
            if (dataFinal.HasValue)
                url += $"dataFinal={dataFinal.Value:yyyy-MM-dd}&";
            else
                url += $"dataFinal={DateTime.Now:yyyy-MM-dd}&";

            var dispositivosResponse = await client.GetAsync(baseUrl + "dispositivos");
            if (dispositivosResponse.IsSuccessStatusCode)
            {
                var json = await dispositivosResponse.Content.ReadAsStringAsync();
                var dispositivos = JsonConvert.DeserializeObject<List<DispositivosModel>>(json);
                ViewBag.Dispositivos = dispositivos.Select(d => new SelectListItem
                {
                    Value = d.IdDispositivo.ToString(),
                    Text = d.Descricao,
                    Selected = dispositivo.HasValue && d.IdDispositivo == dispositivo.Value
                }).ToList();
            }

            var sensoresResponse = await client.GetAsync(baseUrl + "sensors");
            if (sensoresResponse.IsSuccessStatusCode)
            {
                var json = await sensoresResponse.Content.ReadAsStringAsync();
                var sensores = JsonConvert.DeserializeObject<List<SensorsModel>>(json);
                ViewBag.Sensores = sensores.Select(s => new SelectListItem
                {
                    Value = s.IdSensor.ToString(),
                    Text = s.Descricao,
                    Selected = sensor.HasValue && s.IdSensor == sensor.Value
                }).ToList();
            }

            var response = await client.GetAsync(url.TrimEnd('&'));

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var leituras = JsonConvert.DeserializeObject<List<LeiturasModel>>(jsonString);

                if (leituras == null || !leituras.Any())
                {
                    ViewBag.Mensagem = "Nenhuma leitura encontrada para os filtros selecionados.";
                    return View(new { labels = new string[0], data = new decimal[0] });
                }

                var chartData = new
                {
                    labels = leituras.Select(l => l.DataHoraLeitura.ToString("yyyy-MM-dd HH:mm")).ToArray(),
                    data = leituras.Select(l => l.Temperatura).ToArray()
                };

                return View(chartData);
            }

            return View("Error");
        }

    }
}
