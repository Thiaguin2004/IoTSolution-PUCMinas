using IoTSolution.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Configura��o do DbContext
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure o TwilioSettings diretamente usando builder.Configuration
builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("TwilioSettings"));


// Configura��o de autentica��o com cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Defina a rota de login
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/Login"; // P�gina de acesso negado
    });

builder.Services.AddHttpClient();  // Necess�rio para requisi��es HTTP
builder.Services.AddControllersWithViews();  // MVC
builder.Services.AddRazorPages();  // Se estiver usando Razor Pages tamb�m

// Registra o servi�o que ser� executado periodicamente
builder.Services.AddHostedService<AlertasService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();  // Para exibir detalhes de erro no navegador
}
else
{
    app.UseExceptionHandler("/Home/Error");  // Em produ��o, redireciona para uma p�gina de erro gen�rica
    app.UseHsts();  // For�a o uso de HTTPS
}

// Configura��o do pipeline de requisi��o HTTP
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Configura��o de autentica��o e autoriza��o
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Rota padr�o para o MVC

app.MapControllers(); // Roteamento para as APIs

app.Run();
