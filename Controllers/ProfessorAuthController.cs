using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity; 
using Academico.Models;
using Academico.Services;

namespace Academico.Controllers;

public class ProfessorAuthController : Controller
{
    private readonly AcademicoContext _context;
    private readonly PasswordHasher<Professor> _passwordHasherProfessor;  
    private readonly GeradorCodigoService _geradorCodigo;

    public ProfessorAuthController(AcademicoContext context, GeradorCodigoService geradorCodigo)
    {
        _context = context;
        _geradorCodigo = geradorCodigo;
        _passwordHasherProfessor = new PasswordHasher<Professor>();
    }

    // ==========================================
    // TELA DE CADASTRO DO PROFESSOR (GET)
    // ==========================================
    [HttpGet] 
    public IActionResult Cadastro() => View();

    // ==========================================
    // PROCESSA O CADASTRO DO PROFESSOR (POST)
    // ==========================================
    [HttpPost]
    public async Task<IActionResult> Cadastro(CadastroProfessorViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        if (model.Senha != model.ConfirmarSenha)
        {
            ModelState.AddModelError("", "As senhas não conferem.");
            return View(model);
        }

        //LIMPA O CPF LOGO NO CADASTRO (Remove pontos e traços da máscara)
        string cpfLimpo = model.Cpf.Replace(".", "").Replace("-", "").Trim();

        //Verifica duplicidade usando o CPF já limpo
        var professorExiste = await _context.Professores.AnyAsync(p => p.Cpf == cpfLimpo);
        if (professorExiste)
        {
            ModelState.AddModelError("", "Este CPF de professor já está cadastrado.");
            return View(model);
        }

        string siapeGerado = await _geradorCodigo.GerarSiapeUnicoAsync();

        var novoProfessor = new Professor
        {
            Nome = model.Nome,
            Email = model.Email,
            Cpf = cpfLimpo,          //Salva apenas números no banco (Ex: 33322211166)
            UserName = cpfLimpo,     //Salva apenas números no banco (Ex: 33322211166)
            DataNascimento = model.DataNascimento,
            Area = "Não Especificada",
            Siape = siapeGerado
        };

        novoProfessor.PasswordHash = _passwordHasherProfessor.HashPassword(novoProfessor, model.Senha);

        _context.Professores.Add(novoProfessor);
        await _context.SaveChangesAsync();

        TempData["MensagemSucesso"] = $"Professor cadastrado com sucesso! Acesse com seu CPF.";
        return RedirectToAction("Login");
    }
    // ==========================================
    // TELA DE LOGIN DO PROFESSOR (GET)
    // ==========================================
    [HttpGet] 
    public IActionResult Login() => View();

    // ==========================================
    // PROCESSA O LOGIN DO PROFESSOR (POST)
    // ==========================================
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        // Limpa o CPF digitado no login
        string cpfTratado = model.Cpf.Replace(".", "").Replace("-", "").Trim();

        // Busca pelo UserName que agora foi atualizado corretamente pelo ADM
        var professor = await _context.Professores.FirstOrDefaultAsync(p => p.UserName == cpfTratado);

        if (professor == null)
        {
            ModelState.AddModelError("", "Usuário não encontrado com este CPF.");
            return View(model);
        }

        if (string.IsNullOrEmpty(professor.PasswordHash))
        {
            ModelState.AddModelError(string.Empty, "Sua conta possui um erro cadastral (Senha não definida). Por favor, refaça o seu cadastro.");
            return View(model);
        }

        var resultadoSenha = _passwordHasherProfessor.VerifyHashedPassword(professor, professor.PasswordHash, model.Senha);

        if (resultadoSenha == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Senha incorreta.");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, professor.Id.ToString()),
            new Claim(ClaimTypes.Name, professor.Nome),
            new Claim(ClaimTypes.Role, "Professor"),
            new Claim("Siape", professor.Siape) 
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Home"); 
    }

    // ==========================================
    // LOGOUT DO PROFESSOR
    // ==========================================
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["MensagemSucesso"] = "Sessão do professor encerrada.";
        return RedirectToAction("PainelLogin", "AlunoAuth");
    }
}