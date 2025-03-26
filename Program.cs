using IoTSolution.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
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
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

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
