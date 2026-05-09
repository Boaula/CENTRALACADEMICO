using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

public class LoginController : Controller
{
    private readonly IConfiguration _config;

    public LoginController(IConfiguration config) => _config = config;

    public IActionResult Entrar()
    {
        // Informamos o caminho exato partindo da raiz de Views
        return View("~/Views/Admin/Login.cshtml");
    }

    [HttpPost]
    public async Task<IActionResult> Entrar(string email, string senha)
    {
        var adminEmail = _config["AdminAccount:Email"];
        var adminSenha = _config["AdminAccount:Senha"];

        if (email == adminEmail && senha == adminSenha)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "Administrador") };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
            
            return RedirectToAction("Index", "Aluno");
        }

        ViewBag.Erro = "Usuário ou senha inválidos";
        return View("~/Views/Admin/Login.cshtml");
    }

    public async Task<IActionResult> Sair()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Entrar");
    }
}