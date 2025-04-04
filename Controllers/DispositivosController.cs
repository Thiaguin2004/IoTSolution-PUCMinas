﻿using IoTSolution.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using NuGet.DependencyResolver;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

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
    }
}
