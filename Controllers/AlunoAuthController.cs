using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity; // Necessário para o PasswordHasher
using Academico.Models;
using Academico.Services;

namespace Academico.Controllers;

public class AlunoAuthController : Controller
{
    private readonly AcademicoContext _context;
    private readonly GeradorCodigoService _geradorCodigo;
    private readonly PasswordHasher<Aluno> _passwordHasher; // Validador de hash POO do .NET
    private readonly PasswordHasher<Professor> _passwordHasherProfessor;  // Validador de hash POO do .NET

    public AlunoAuthController(AcademicoContext context, GeradorCodigoService geradorCodigo)
    {
        _context = context;
        _geradorCodigo = geradorCodigo;
        _passwordHasher = new PasswordHasher<Aluno>();
        _passwordHasherProfessor = new PasswordHasher<Professor>();
    }

    // ==========================================
    // TELA DE LOGIN
    // ==========================================
    [HttpGet] public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        // Busca o aluno diretamente via DbContext usando o CPF (UserName)
        var aluno = await _context.Alunos.FirstOrDefaultAsync(a => a.UserName == model.Matricula);

        if (aluno == null)
        {
            ModelState.AddModelError("", "Usuário não encontrado com este CPF.");
            return View(model);
        }

        // Verifica se a senha digitada bate com o PasswordHash do banco
        var resultadoSenha = _passwordHasher.VerifyHashedPassword(aluno, aluno.PasswordHash!, model.Senha);

        if (resultadoSenha == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError("", "Senha incorreta.");
            return View(model);
        }

        // Cria a identidade do Cookie manualmente
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, aluno.Id.ToString()),
            new Claim(ClaimTypes.Name, aluno.Nome),
            new Claim("Matricula", aluno.Matricula)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        // Loga usando o esquema padrão de Cookies
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Home");
    }

    // ==========================================
    // TELA DE CADASTRO
    // ==========================================
    [HttpGet] public IActionResult Cadastro() => View();

    [HttpPost]
    public async Task<IActionResult> Cadastro(CadastroViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var validador = new ValidadorSenha(model.Senha, model.ConfirmarSenha);
        if (!validador.EhValida)
        {
            foreach (var erro in validador.Erros) ModelState.AddModelError("", erro);
            return View(model);
        }

        // Verifica duplicidade usando o DbContext puro
        var alunoExiste = await _context.Alunos.AnyAsync(a => a.UserName == model.Cpf);
        if (alunoExiste)
        {
            ModelState.AddModelError("", "Este CPF já está cadastrado.");
            return View(model);
        }

        string matriculaGerada = await _geradorCodigo.GerarMatriculaUnicaAsync();

        var novoAluno = new Aluno
        {
            Nome = model.Nome,
            Cpf = model.Cpf,
            DataNascimento = model.DataNascimento,
            Matricula = matriculaGerada,
            Curso = "Pendente",
            UserName = model.Cpf,
            Senha = "manual_managed",
            SecurityStamp = Guid.NewGuid().ToString() // Mantém a coluna do banco preenchida
        };

        // Criptografa a senha antes de salvar na coluna PasswordHash
        novoAluno.PasswordHash = _passwordHasher.HashPassword(novoAluno, model.Senha);

        _context.Alunos.Add(novoAluno);
        await _context.SaveChangesAsync();

        TempData["MensagemSucesso"] = $"Cadastro realizado! Use seu CPF para logar. Sua matrícula é: {matriculaGerada}";
        return RedirectToAction("Login", "AlunoAuth");
    }

    // ==========================================
    // LOGOUT
    // ==========================================
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("PainelLogin", "AlunoAuth");
    }

    // ==========================================
    // TELA SELEÇÃO DE PORTAL DE LOGIN (GET)
    // ==========================================
    [HttpGet]
    public IActionResult PainelLogin()
    {
        return View();
    }
}