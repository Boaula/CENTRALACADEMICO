using Academico.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Academico.Services; // <-- O namespace aponta para a nova pasta

public class GeradorCodigoService
{
    private readonly AcademicoContext _context;
    private static readonly Random _random = new Random();

    public GeradorCodigoService(AcademicoContext context)
    {
        _context = context;
    }

    public async Task<string> GerarCodigoDiarioUnicoAsync()
    {
        int totalDisciplinas = await _context.Disciplinas.CountAsync();
        int anoAtual = DateTime.Now.Year;
        
        string codigoGerado;
        bool codigoJaExiste;

        do
        {
            codigoGerado = $"{anoAtual}{totalDisciplinas}{_random.Next(0, 99)}";
            codigoJaExiste = await _context.Disciplinas.AnyAsync(d => d.CodigoDiario == codigoGerado);
        } while (codigoJaExiste);

        return codigoGerado;
    }

// ==========================================
    // MÉTODO NOVO: GERAR CÓDIGO DA DISCIPLINA (Ex: 53469)
    // ==========================================
    public async Task<int> GerarCodigoDisciplinaUnicoAsync()
    {
        int codigoGerado;
        bool codigoJaExiste;

        do
        {
            // Sorteia um número de 5 dígitos entre 10000 e 99999
            codigoGerado = _random.Next(10000, 100000);
            
            // Verifica no banco se esse número de disciplina já existe
            codigoJaExiste = await _context.Disciplinas.AnyAsync(d => d.CodigoDisciplina == codigoGerado);

        } while (codigoJaExiste);

        return codigoGerado;
    }

    // ==========================================
    // MÉTODO NOVO: GERAR MATRÍCULA ÚNICA DO ALUNO
    // ==========================================
    public async Task<string> GerarMatriculaUnicaAsync()
    {
        int anoAtual = DateTime.Now.Year;
        string matriculaGerada;
        bool matriculaExiste;

        do
        {
            // Gera um número aleatório de 6 dígitos entre 100000 e 999999
            int sulfixoAleatorio = _random.Next(100000, 1000000);
            
            // Junta o ano atual com o número (Ex: "2026" + "584319" = "2026584319")
            matriculaGerada = $"{anoAtual}{sulfixoAleatorio}";
            
            // Verifica na tabela de Alunos se essa matrícula já existe
            matriculaExiste = await _context.Alunos.AnyAsync(a => a.Matricula == matriculaGerada);

        } while (matriculaExiste);

        return matriculaGerada;
    }
}