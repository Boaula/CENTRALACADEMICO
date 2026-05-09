using Microsoft.AspNetCore.Mvc;
using Academico.Models;
using Academico.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Academico.Controllers;

public class ProfessorController : Controller
{
    readonly IProfessorRepository _professorRepository;

    public ProfessorController(IProfessorRepository professorRepository)
    {
        _professorRepository = professorRepository;
    }

    public async Task<IActionResult> Index()
    {
        var professores = await _professorRepository.GetAllProfessores();
        return View(professores);
    }

    [Authorize] // Bloqueia tudo para quem não está logado
    public IActionResult CriarProfessor()
    {
        return View();
    }

    [HttpPost]
    [Authorize] // Bloqueia tudo para quem não está logado
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

    public IActionResult AtualizarProfessor()
    {
        return View();
    }

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