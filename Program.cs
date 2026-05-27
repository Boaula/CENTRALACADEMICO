using Academico.Models;
using Academico.Repositories;
using Academico.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ---- CONFIGURAÇÃO DE SERVIÇOS (CONTAINER) ----

builder.Services.AddControllersWithViews(); // Duplicação removida aqui

// Seus Repositories e Services
builder.Services.AddScoped<IHomeRepository, HomeRepository>();
builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
builder.Services.AddScoped<IProfessorRepository, ProfessorRepository>();
builder.Services.AddScoped<IDisciplinaRepository, DisciplinaRepository>();
builder.Services.AddScoped<GeradorCodigoService>();

// Conexão com o Banco de Dados
builder.Services.AddDbContext<AcademicoContext>(options => 
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

//--------------------SISTEMA DE AUTENTICIDADE-------------------\\

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Rota padrão (pode ser a do Aluno ou do ADM, o .NET usará esta se nenhuma for especificada)
        options.LoginPath = "/AlunoAuth/PainelLogin"; 
        options.AccessDeniedPath = "/AlunoAuth/PainelLogin"; 
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

// --------------------END AUTENTICAÇÃO-----------------------\\


// ---- CONSTRUÇÃO DO APLICATIVO ----
// O "var app" DEVE vir obrigatoriamente aqui, antes de qualquer configuração com "app."
var app = builder.Build();


// ---- CONFIGURAÇÃO DO PIPELINE (MIDDLEWARES) ----

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

// Importante: Mantém o arquivos físicos (CSS/JS) acessíveis
app.UseStaticFiles(); 

app.UseRouting();

// MIDDLEWARES DE SEGURANÇA (Apenas uma vez, logo após o UseRouting)
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();