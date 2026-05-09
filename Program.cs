using Academico.Models;
using Academico.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IHomeRepository, HomeRepository>();
builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
builder.Services.AddScoped<IProfessorRepository, ProfessorRepository>();
builder.Services.AddDbContext<AcademicoContext>
    (options => options.UseMySql(connectionString,
        ServerVersion.AutoDetect(connectionString))
    );

//--------------------SISTEMA DE AUTENTICIDADE-------------------\\

// 1. Configurar o serviço de Cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Entrar"; // Onde o ADM vai logar
        options.AccessDeniedPath = "/Login/AcessoNegado";
    });

var app = builder.Build();

// 2. Habilitar o Middleware (DEVE vir antes do MapControllerRoute)
app.UseAuthentication();
app.UseAuthorization();

// --------------------END AUTENTICAÇÃO-----------------------\\


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
