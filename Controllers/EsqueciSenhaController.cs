using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Academico.Services;
using Academico.Models;
using Academico.Models.ResetSenha;

public class EsqueciSenhaController : Controller
{
    private readonly AcademicoContext _context;
    private readonly IEmailSender _emailSender;
    private readonly PasswordHasher<Aluno> _passwordHasherAluno;
    private readonly PasswordHasher<Professor> _passwordHasherProfessor;

    public EsqueciSenhaController(
        AcademicoContext context,
        IEmailSender emailSender)
    {
        _context = context;
        _emailSender = emailSender;
        _passwordHasherAluno = new PasswordHasher<Aluno>();
        _passwordHasherProfessor = new PasswordHasher<Professor>();
    }

    [HttpGet]
    public IActionResult Solicitar()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Solicitar(EsqueciSenhaViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var aluno = await _context.Alunos
            .FirstOrDefaultAsync(a => a.UserName == model.Email || a.Email == model.Email);

        var professor = await _context.Professores
            .FirstOrDefaultAsync(p => p.UserName == model.Email || p.Email == model.Email);

        if (aluno == null && professor == null)
        {
            ModelState.AddModelError("", "Usuário não encontrado.");
            return View(model);
        }

        var token = Guid.NewGuid().ToString("N");
        var expira = DateTime.UtcNow.AddHours(2);

        if (aluno != null)
        {
            aluno.ResetSenhaToken = token;
            aluno.ResetSenhaTokenExpiraEm = expira;
        }
        else if (professor != null)
        {
            professor.ResetSenhaToken = token;
            professor.ResetSenhaTokenExpiraEm = expira;
        }

        await _context.SaveChangesAsync();

        var email = aluno?.Email ?? professor?.Email;
        if (!string.IsNullOrEmpty(email))
        {
            var link = Url.Action(
                "ResetarSenha",
                "EsqueciSenha",
                new { token },
                Request.Scheme);

            var corpo = $"Clique aqui para redefinir sua senha: <a href=\"{link}\">Redefinir senha</a>";
            await _emailSender.SendEmailAsync(email, "Recuperação de senha", corpo);
        }

        TempData["Mensagem"] = "Se o usuário existir, um link de recuperação será enviado por e-mail.";
        return RedirectToAction("Solicitar");
    }

    [HttpGet]
    public IActionResult ResetarSenha(string token)
    {
        if (string.IsNullOrEmpty(token)) return BadRequest();

        return View(new ResetSenhaViewModel { Token = token });
    }

    [HttpPost]
    public async Task<IActionResult> ResetarSenha(ResetSenhaViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        if (model.NovaSenha != model.ConfirmarSenha)
        {
            ModelState.AddModelError("", "As senhas não conferem.");
            return View(model);
        }

        var aluno = await _context.Alunos
            .FirstOrDefaultAsync(a => a.ResetSenhaToken == model.Token && a.ResetSenhaTokenExpiraEm >= DateTime.UtcNow);

        var professor = await _context.Professores
            .FirstOrDefaultAsync(p => p.ResetSenhaToken == model.Token && p.ResetSenhaTokenExpiraEm >= DateTime.UtcNow);

        if (aluno == null && professor == null)
        {
            ModelState.AddModelError("", "Link inválido ou expirado.");
            return View(model);
        }

        if (aluno != null)
        {
            aluno.PasswordHash = _passwordHasherAluno.HashPassword(aluno, model.NovaSenha);
            aluno.ResetSenhaToken = null;
            aluno.ResetSenhaTokenExpiraEm = null;
        }
        else if (professor != null)
        {
            professor.PasswordHash = _passwordHasherProfessor.HashPassword(professor, model.NovaSenha);
            professor.ResetSenhaToken = null;
            professor.ResetSenhaTokenExpiraEm = null;
        }

        await _context.SaveChangesAsync();

        TempData["MensagemSucesso"] = "Senha alterada com sucesso.";
        return RedirectToAction("Login", "AlunoAuth");
    }
}