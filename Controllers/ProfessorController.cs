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
    
    [ApenasAdmin]
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

    [ApenasAdmin]

    public IActionResult CriarProfessor()
    {
        return View();
    }

    [ApenasAdmin]
    [HttpPost]
    public async Task<IActionResult> CriarProfessorAsync(Professor professor)
    {
        if(await _professorRepository.CriarProfessorAsync(professor))
        {
            TempData["Tipo"] = "success";
            TempData["Mensagem"] = $"Professor {professor.Nome} Cadastrado com sucesso";
        } else
        {
            TempData["Tipo"] = "danger";
            TempData["Mensagem"] = $"Professor {professor.Nome} Cadastrado com sucesso";
        }
        //await _professorRepository.CriarProfessorAsync(professor);
        return RedirectToAction("CriarProfessor");
    }

    [ApenasAdmin]
    [HttpGet]
    public async Task<IActionResult> AtualizarProfessor(int id)
    {
        var professor = await _professorRepository.BuscarPorIdAsync(id);
        if (professor == null) return NotFound();

        return View(professor);
    }

    [ApenasAdmin]
    [HttpPost]
    public async Task<IActionResult> AtualizarProfessorAsync(Professor professor)
    {
        if(await _professorRepository.AtualizarProfessorAsync(professor))
        {
            TempData["Tipo"] = "success";
            TempData["Mensagem"] = $"Professor {professor.Nome} atualizado com sucesso!";
        } else
        {
            TempData["Tipo"] = "danger";
            TempData["Mensagem"] = $"Professor {professor.Nome} não atualizado!";
        }
        return RedirectToAction("Atualizarprofessor");
    }
    
    [ApenasAdmin]
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