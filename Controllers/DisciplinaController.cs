// Cadastrar disciplina com uma listView puxando a lista de professores para ser selecionado

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Academico.Models;
using Academico.Repositories; // <-- ADICIONADO: Agora o C# sabe onde encontrar as interfaces
using Microsoft.AspNetCore.Authorization;
using Academico.Services;

namespace Academico.Controllers;

[Authorize] // Bloqueia tudo para quem não está logado
public class DisciplinaController : Controller
{
    private readonly IProfessorRepository _professorRepository;
    private readonly IDisciplinaRepository _disciplinaRepository;

    // O construtor agora vai receber a injeção de dependência sem erros
    public DisciplinaController(IProfessorRepository professorRepository, IDisciplinaRepository disciplinaRepository)
    {
        _professorRepository = professorRepository;
        _disciplinaRepository = disciplinaRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // Busca todas as disciplinas usando o seu repositório
        var Disciplina = await _disciplinaRepository.GetAllDisciplinasAsync(); 
        
        // Passa a lista de disciplinas para a View de Cards
        return View(Disciplina);
    }

    [ApenasAdmin]
    [HttpGet]
    public async Task<IActionResult> CriarDisciplina()
    {
        // Busca os professores de forma assíncrona para listar no dropdown
        var professores = await _professorRepository.GetAllProfessores();

        // GERAR LISTA DE PERÍODOS
        int anoAtual = DateTime.Now.Year;
        var periodos = new List<string> //VAR para o Select
            {
                $"01 --> {anoAtual - 1}",
                $"02 --> {anoAtual - 1}",
                $"01 --> {anoAtual}",
                $"02 --> {anoAtual}",
                $"01 --> {anoAtual + 1}"
            };

        var grau = new List<string> //VAR para o Select
        {
            $"Tecnico",
            $"Graduação",
            $"Pós-Graduação",
            $"Mestrado",
            $"Doutorado"
        };
        ViewBag.Periodos = new SelectList(periodos);
        ViewBag.Grau = new SelectList(grau);

        
        ViewBag.Professor = new SelectList(professores, "Id", "Nome");
        
        // Forçamos o retorno da View com o nome exato do arquivo .cshtml
        return View("CriarDisciplina");
    }

    [ApenasAdmin]
    [HttpPost]
    public async Task<IActionResult> CriarDisciplina(DisciplinaViewModel model)
    {
        ModelState.Remove("CodigoDiario");
        ModelState.Remove("CodigoDisciplina");
    foreach (var item in ModelState)
    {
        foreach (var error in item.Value.Errors)
        {
            Console.WriteLine($"CAMPO: {item.Key}");
            Console.WriteLine($"ERRO: {error.ErrorMessage}");
        }
    }
        // 1. VERIFICAÇÃO DE SEGURANÇA: Se o formulário veio com campos vazios...
        if (!ModelState.IsValid)
        {
            // BUSCA NOVAMENTE OS PROFESSORES PARA RECOMPOR A LISTA
            var professores = await _professorRepository.GetAllProfessores();

            // RECRIA AS LISTAS EXATAMENTE IGUAL AO SEU GET
            int anoAtual = DateTime.Now.Year;
            var periodos = new List<string>
            {
                $"01 --> {anoAtual - 1}",
                $"02 --> {anoAtual - 1}",
                $"01 --> {anoAtual}",
                $"02 --> {anoAtual}",
                $"01 --> {anoAtual + 1}"
            };

            var grau = new List<string>
            {
                "Tecnico",
                "Graduação",
                "Pós-Graduação",
                "Mestrado",
                "Doutorado"
            };

            // REABASTECE AS VIEWBAGS PARA A TELA NÃO RECARREGAR QUEBRADA
            ViewBag.Periodos = new SelectList(periodos);
            ViewBag.Grau = new SelectList(grau);
            ViewBag.Professor = new SelectList(professores, "Id", "Nome");

            // INTERROMPE O ENVIO E DEVOLVE O USUÁRIO PARA A TELA MOSTRANDO OS ERROS
            return View("CriarDisciplina", model);
        }

        // 2. SE PASSOU NA VALIDAÇÃO, CONTINUA O CADASTRO NORMALMENTE
        var novaDisciplina = new Disciplina
        {
            Periodo = model.Periodo,
            Nome = model.Nome,
            Grau = model.Grau,
            CargaHoraria = model.CargaHoraria,
            TotalAulas = model.TotalAulas,
            Turno = model.Turno,
            QuantidadeEtapas = model.QuantidadeEtapas,
            Horario = model.Horario,
            LocalAula = model.LocalAula,
            ProfessorId = model.ProfessorId
        };

        await _disciplinaRepository.AdicionarAsync(novaDisciplina);
        
        return RedirectToAction("CriarDisciplina");
    }

    
    [ApenasAdmin]
    // ROTA 1 - GET: Disciplina/EditarDisciplina (Sem ID)
    // Mostra a lista de cards com os botões para o ADM
    [HttpGet]
    public async Task<IActionResult> EditarDisciplina()
    {
        var disciplinas = await _disciplinaRepository.GetAllDisciplinasAsync();
        
        // Passamos a lista de disciplinas. Na View, vamos checar se recebemos uma lista ou um formulário
        return View("EditarDisciplina", disciplinas);
    }


    [ApenasAdmin]
    // ROTA 2 - GET: Disciplina/EditarDisciplina/5 (Com ID)
    // Mostra o formulário de edição da disciplina selecionada
    [HttpGet]
    [Route("Disciplina/EditarDisciplina/{id:int}")]
    public async Task<IActionResult> EditarDisciplina(int id)
    {
        var disciplina = await _disciplinaRepository.BuscarPorIdAsync(id);
        if (disciplina == null) return NotFound();

        var model = new DisciplinaViewModel
        {
            Id = disciplina.Id,
            Periodo = disciplina.Periodo,
            Nome = disciplina.Nome,
            Grau = disciplina.Grau,
            Turno = disciplina.Turno,
            CargaHoraria = disciplina.CargaHoraria,
            TotalAulas = disciplina.TotalAulas,
            QuantidadeEtapas = disciplina.QuantidadeEtapas,
            Horario = disciplina.Horario,
            LocalAula = disciplina.LocalAula,
            ProfessorId = disciplina.ProfessorId,
            CodigoDiario = disciplina.CodigoDiario,
            CodigoDisciplina = disciplina.CodigoDisciplina
        };

        var professores = await _professorRepository.GetAllProfessores();
        int anoAtual = DateTime.Now.Year;
        var periodos = new List<string> { 
                $"01 --> {anoAtual - 1}",
                $"02 --> {anoAtual - 1}",
                $"01 --> {anoAtual}",
                $"02 --> {anoAtual}",
                $"01 --> {anoAtual + 1}"}; // Simplificado para o exemplo
        var grau = new List<string> { "Tecnico", "Graduação", "Pós-Graduação", "Mestrado", "Doutorado" };

        ViewBag.Periodos = new SelectList(periodos, model.Periodo);
        ViewBag.Grau = new SelectList(grau, model.Grau);
        ViewBag.Professor = new SelectList(professores, "Id", "Nome", model.ProfessorId);

        // Passamos o modelo único de edição
        return View("EditarDisciplina", model);
    }

 
    [ApenasAdmin]
    // POST: Disciplina/EditarDisciplina
    [HttpPost]
    public async Task<IActionResult> EditarDisciplina(DisciplinaViewModel model)
    {
        ModelState.Remove("CodigoDiario");
        ModelState.Remove("CodigoDisciplina");
        if (!ModelState.IsValid)
        {
            var professores = await _professorRepository.GetAllProfessores();
            int anoAtual = DateTime.Now.Year;
            var periodos = new List<string> { $"01 --> {anoAtual}", $"02 --> {anoAtual}" };
            var grau = new List<string> { "Tecnico", "Graduação", "Pós-Graduação", "Mestrado", "Doutorado" };

            ViewBag.Periodos = new SelectList(periodos, model.Periodo);
            ViewBag.Grau = new SelectList(grau, model.Grau);
            ViewBag.Professor = new SelectList(professores, "Id", "Nome", model.ProfessorId);

            return View("EditarDisciplina", model);
        }

        // Busca a entidade ativa do banco para não perder os códigos automáticos (Diário/Disciplina)
        var disciplinaExistente = await _disciplinaRepository.BuscarPorIdAsync(model.Id);
        if (disciplinaExistente == null) return NotFound();

        // Atualiza apenas os dados modificáveis vindos do formulário
        disciplinaExistente.Periodo = model.Periodo;
        disciplinaExistente.Nome = model.Nome;
        disciplinaExistente.Grau = model.Grau;
        disciplinaExistente.Turno = model.Turno;
        disciplinaExistente.CargaHoraria = model.CargaHoraria;
        disciplinaExistente.TotalAulas = model.TotalAulas;
        disciplinaExistente.QuantidadeEtapas = model.QuantidadeEtapas;
        disciplinaExistente.Horario = model.Horario;
        disciplinaExistente.LocalAula = model.LocalAula;
        disciplinaExistente.ProfessorId = model.ProfessorId;

        await _disciplinaRepository.AtualizarAsync(disciplinaExistente); // Certifique-se de ter o método de Update no seu repositório
        
        return RedirectToAction("EditarDisciplina", "Disciplina", new { id = "" });
    }

    
    [ApenasAdmin]
    // POST: Disciplina/ExcluirDisciplina
    [HttpPost]
    public async Task<IActionResult> ExcluirDisciplina(int id)
    {
        await _disciplinaRepository.ExcluirAsync(id); // Certifique-se de ter o método de Delete no seu repositório
        return RedirectToAction("EditarDisciplina");
    }

}