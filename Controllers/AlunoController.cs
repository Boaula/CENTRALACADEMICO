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
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var alunos = await _alunoRepository.GetAllAlunos();
        return View(alunos);
    }

    [AllowAnonymous]
    public async Task<IActionResult> AlunosPublic()
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

    // 1. ESTE É O QUE BUSCA OS DADOS (Faltava este)
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> AtualizarAluno(int id)
    {
        // Usamos o método que acabamos de criar no repositório
        var aluno = await _alunoRepository.BuscarPorIdAsync(id);

        if (aluno == null)
        {
            return NotFound(); // Caso o ID não exista no banco
        }

        return View(aluno); // Aqui a mágica acontece: os dados vão para o Form
    }

    [HttpPost]
    [Authorize]
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

    [Authorize]
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