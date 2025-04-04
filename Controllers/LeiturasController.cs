using IoTSolution.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using NuGet.DependencyResolver;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;

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

        public async Task<IActionResult> Index(int? dispositivo, int? sensor, DateTime? dataInicial, DateTime? dataFinal, string? temperatura)
        {
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            var client = _httpClientFactory.CreateClient();

            decimal? temperaturaDecimal = null;
            if (!string.IsNullOrEmpty(temperatura))
            {
                temperatura = temperatura.Replace(',', '.');
                if (decimal.TryParse(temperatura, NumberStyles.Any, new CultureInfo("en-US"), out var temp))
                {
                    temperaturaDecimal = temp;
                }
            }

            var url = $"{baseUrl}leituras?";

            if (dispositivo.HasValue)
                url += $"dispositivo={dispositivo}&";

            if (sensor.HasValue)
                url += $"sensor={sensor}&";

            if (dataInicial.HasValue)
                url += $"dataInicial={dataInicial.Value:yyyy-MM-dd}&";

            if (dataFinal.HasValue)
                url += $"dataFinal={dataFinal.Value:yyyy-MM-dd}&";

            if (temperaturaDecimal.HasValue)
                url += $"temperatura={Convert.ToString(temperatura, new CultureInfo("en-US"))}&";

            url = url.TrimEnd('&');
            url = url.TrimEnd('?');

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var leituras = JsonConvert.DeserializeObject<List<LeiturasModel>>(jsonString);

                return View(leituras);
            }

            return View("Error");
        }
    }
}
