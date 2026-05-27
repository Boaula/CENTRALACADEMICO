using Academico.Models;
using Academico.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Academico.Services;

namespace Academico.Controllers;

[Authorize] // Bloqueia tudo para quem não está logado
public class AlunoController : Controller
{
    readonly IAlunoRepository _alunoRepository;

    public AlunoController(IAlunoRepository alunoRepository)
    {
        _alunoRepository = alunoRepository;
    }

    [Authorize(Roles = "Admin")] // 🚨 APENAS usuários com a Role "Admin" passam daqui
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var alunos = await _alunoRepository.GetAllAlunos();
        return View(alunos);
    }

    public async Task<IActionResult> AlunosPublic()
    {
        var alunos = await _alunoRepository.GetAllAlunos();
        return View(alunos); 
    }

    [Authorize(Roles = "Admin")]
    public IActionResult CriarAluno()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
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
    [Authorize(Roles = "Admin")]
    [HttpGet]
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

    [Authorize(Roles = "Admin")]
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

    [Authorize(Roles = "Admin")]
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