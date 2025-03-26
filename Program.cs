using IoTSolution.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Configuração de autenticação com cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Defina a rota de login
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/Login"; // Página de acesso negado
    });


builder.Services.AddHttpClient();  // Necessário para requisições HTTP
builder.Services.AddControllersWithViews();  // MVC
builder.Services.AddRazorPages();  // Se estiver usando Razor Pages também

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();  // Para exibir detalhes de erro no navegador
}
else
{
    app.UseExceptionHandler("/Home/Error");  // Em produção, redireciona para uma página de erro genérica
    app.UseHsts();  // Força o uso de HTTPS
}
// Configuração do pipeline de requisição HTTP
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

// Configuração de autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Rota padrão para o MVC

app.MapControllers(); // Roteamento para as APIs

app.Run();
