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
    [HttpPost]
    public async Task<IActionResult> CriarAlunoAsync(Aluno aluno)
    {
        if (aluno == null) return BadRequest();

        //BLINDAGEM: Limpa o CPF enviado pelo formulário do ADM
        string cpfLimpo = aluno.Cpf.Replace(".", "").Replace("-", "").Trim();
        
        aluno.Cpf = cpfLimpo;
        aluno.UserName = cpfLimpo; //Essencial para o login funcionar depois!
        aluno.SecurityStamp = Guid.NewGuid().ToString();
        aluno.Curso ??= "Pendente"; // Evita nulos se não for preenchido na tela
        aluno.Senha = "manual_managed";

        //GERAR MATRÍCULA ÚNICA (Se o seu repositório já não fizer isso)
        // Se o repositório não gera a matrícula automaticamente, gere aqui:
        //aluno.Matricula = await _geradorCodigo.GerarMatriculaUnicaAsync();

        // 3. CRIPTOGRAFAR A SENHA INICIAL
        // Se o ADM define uma senha (ex: aluno.PasswordHash vindo de um input 'Senha'), criptografe-a:
        if (!string.IsNullOrEmpty(aluno.PasswordHash))
        {
            var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<Aluno>();
            aluno.PasswordHash = passwordHasher.HashPassword(aluno, aluno.PasswordHash);
        }

        // Envia o objeto totalmente tratado para o repositório salvar
        if (await _alunoRepository.CriarAlunoAsync(aluno))
        {
            TempData["Tipo"] = "success";
            TempData["Mensagem"] = $"Aluno {aluno.Nome} cadastrado com sucesso!";
        } 
        else
        {
            TempData["Tipo"] = "danger";
            TempData["Mensagem"] = $"Erro ao cadastrar o aluno {aluno.Nome}.";
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
        if (aluno == null) return BadRequest();

        //ADM: Limpa o CPF enviado no formulário de edição
        string cpfLimpo = aluno.Cpf.Replace(".", "").Replace("-", "").Trim();
        
        aluno.Cpf = cpfLimpo;
        aluno.UserName = cpfLimpo; //Sincroniza o UserName do banco com o novo CPF

        if (await _alunoRepository.AtualizarAlunoAsync(aluno))
        {
            TempData["Tipo"] = "success";
            TempData["Mensagem"] = $"Aluno {aluno.Nome} atualizado com sucesso!";
        } 
        else
        {
            TempData["Tipo"] = "danger";
            TempData["Mensagem"] = $"Aluno {aluno.Nome} não atualizado!";
        }
        
        return RedirectToAction("Index"); // Ajuste o redirecionamento para a sua View de listagem se necessário
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