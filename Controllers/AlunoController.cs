using Academico.Models;
using Academico.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Academico.Controllers;

public class AlunoController : Controller
{
    readonly IAlunoRepository _alunoRepository;

    public AlunoController(IAlunoRepository alunoRepository)
    {
        _alunoRepository = alunoRepository;
    }

    public async Task<IActionResult> Index()
    {
        var alunos = await _alunoRepository.GetAllAlunos();
        return View(alunos);
    }

    [Authorize] // Bloqueia tudo para quem não está logado
    public IActionResult CriarAluno()
    {
        return View();
    }

    [HttpPost]
    [Authorize] // Bloqueia tudo para quem não está logado
    public async Task<IActionResult> CriarAlunoAsync(Aluno aluno)
    {
        if(await _alunoRepository.CriarAlunoAsync(aluno))
        {
            TempData["Tipo"] = "success";
            TempData["Mensagem"] = $"Aluno {aluno.Nome} Cadastrado com sucesso";
        } else
        {
            TempData["Tipo"] = "danger";
            TempData["Mensagem"] = $"Aluno {aluno.Nome} Cadastrado com sucesso";
        }
        return RedirectToAction("CriarAluno");
    }

       public IActionResult AtualizarAluno()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AtualizarAlunoAsync(Aluno aluno)
    {
        if(await _alunoRepository.AtualizarAlunoAsync(aluno))
        {
            TempData["Tipo"] = "success";
            TempData["Mensagem"] = $"Aluno {aluno.Nome} atualizado com sucesso!";
        } else
        {
            TempData["Tipo"] = "danger";
            TempData["Mensagem"] = $"Aluno {aluno.Nome} não atualizado!";
        }
        return RedirectToAction("AtualizarAluno");
    }
        public async Task<IActionResult> ExcluirAlunoAsync(int Id)
    {
        if(await _alunoRepository.ExcluirAlunoAsync(Id))
        {
            TempData["Tipo"] = "success";
            TempData["Mensagem"] = $"Aluno Excluido com sucesso!";
        } else
        {
            TempData["Tipo"] = "danger";
            TempData["Mensagem"] = $"Aluno não Excluido!";
        }
        return RedirectToAction("Index");
    }
}