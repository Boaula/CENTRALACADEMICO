using Microsoft.AspNetCore.Mvc;
using Academico.Models;
using Academico.Repositories;
using Microsoft.AspNetCore.Authorization;
using Academico.Services;

namespace Academico.Controllers;
    
[Authorize] // Bloqueia tudo para quem não está logado
public class ProfessorController : Controller
{
    readonly IProfessorRepository _professorRepository;

    public ProfessorController(IProfessorRepository professorRepository)
    {
        _professorRepository = professorRepository;
    }
    
    [Authorize(Roles = "Admin")]  //APENAS usuários com a Role "Admin" passam daqui
    public async Task<IActionResult> Index()
    {
        var professores = await _professorRepository.GetAllProfessores();
        return View(professores);
    }
    
    public async Task<IActionResult> ProfessoresPublic()
    {
        var professores = await _professorRepository.GetAllProfessores();
        return View(professores); 
    }

    [Authorize(Roles = "Admin")] 
    public IActionResult CriarProfessor()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]     [HttpPost]
    [HttpPost]
    public async Task<IActionResult> CriarProfessorAsync(Professor professor)
    {
        if (professor == null) return BadRequest();

        //BLINDAGEM: Limpa o CPF enviado pelo formulário do ADM para o Professor
        string cpfLimpo = professor.Cpf.Replace(".", "").Replace("-", "").Trim();
        
        professor.Cpf = cpfLimpo;
        professor.UserName = cpfLimpo; //Essencial para o login funcionar depois!
        professor.Area ??= "Não Especificada"; // Evita nulos se não for preenchido na tela

        //GERAR SIAPE ÚNICO (Caso seu repositório ainda não faça isso)
        // Se o repositório não gerar automaticamente, injete o serviço e gere aqui:
        // professor.Siape = await _geradorCodigo.GerarSiapeUnicoAsync();

        //CRIPTOGRAFAR A SENHA INICIAL COM O HASHER DE PROFESSOR
        if (!string.IsNullOrEmpty(professor.PasswordHash))
        {
            var passwordHasherProfessor = new Microsoft.AspNetCore.Identity.PasswordHasher<Professor>();
            professor.PasswordHash = passwordHasherProfessor.HashPassword(professor, professor.PasswordHash);
        }

        // Envia o objeto totalmente tratado para o repositório de professores salvar
        if (await _professorRepository.CriarProfessorAsync(professor))
        {
            TempData["Tipo"] = "success";
            TempData["Mensagem"] = $"Professor {professor.Nome} cadastrado com sucesso!";
        } 
        else
        {
            TempData["Tipo"] = "danger";
            TempData["Mensagem"] = $"Erro ao cadastrar o professor {professor.Nome}.";
        }
        
        return RedirectToAction("CriarProfessor");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> AtualizarProfessor(int id)
    {
        var professor = await _professorRepository.BuscarPorIdAsync(id);
        if (professor == null) return NotFound();

        return View(professor);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AtualizarProfessorAsync(Professor professor)
    {
        if (professor == null) return BadRequest();

        string cpfLimpo = professor.Cpf.Replace(".", "").Replace("-", "").Trim();
        
        professor.Cpf = cpfLimpo;
        
        professor.UserName = cpfLimpo;

        if (await _professorRepository.AtualizarProfessorAsync(professor))
        {
            TempData["Tipo"] = "success";
            TempData["Mensagem"] = $"Professor {professor.Nome} atualizado com sucesso! Novo CPF: {professor.Cpf}";
        } 
        else
        {
            TempData["Tipo"] = "danger";
            TempData["Mensagem"] = $"Professor {professor.Nome} não atualizado!";
        }
        
        return RedirectToAction("Index"); // Geralmente redireciona para a lista após editar
    }
    
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ExcluirProfessorAsync(int Id)
    {
        if(await _professorRepository.ExcluirProfessorAsync(Id))
        {
            TempData["Tipo"] = "success";
            TempData["Mensagem"] = $"Professor Excluido com sucesso!";
        } else
        {
            TempData["Tipo"] = "danger";
            TempData["Mensagem"] = $"Professor não Excluido!";
        }
        return RedirectToAction("Index");
    }
}