﻿using IoTSolution.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using NuGet.DependencyResolver;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;

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
        public IActionResult Create()
        {
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
            var response = await client.GetAsync(baseUrl + "sensors/" + id);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var sensor = JsonConvert.DeserializeObject<SensorsModel>(jsonString);
                return View(sensor);
            }

            return View("Error");
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
