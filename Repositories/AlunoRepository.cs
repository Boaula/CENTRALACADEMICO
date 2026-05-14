using Academico.Models;
using Microsoft.EntityFrameworkCore;

namespace Academico.Repositories;

public class AlunoRepository : IAlunoRepository
{
    readonly AcademicoContext _context;

    public AlunoRepository(AcademicoContext context)
    {
        _context = context;
    }
    public async Task<bool> CriarAlunoAsync(Aluno aluno)
    {
        aluno.Matricula = $"2026001{new Random().Next(0, 99)}";
        await _context.AddAsync(aluno);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Aluno>> GetAllAlunos()
    {
        return await _context.Alunos.ToListAsync();
    }

    public async Task<bool> AtualizarAlunoAsync(Aluno aluno)
    {
        var alunoBanco = await _context.Alunos.FirstOrDefaultAsync(x => x.Id == aluno.Id);
        alunoBanco!.Nome = aluno.Nome;
        alunoBanco.Cpf = aluno.Cpf;
        alunoBanco.Curso = aluno.Curso;
        alunoBanco.DataNascimento = aluno.DataNascimento;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Aluno> BuscarPorIdAsync(int id)
    {
        // O FirstOrDefaultAsync busca o aluno que tem o ID igual ao que clicamos na lista
        return await _context.Alunos.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<bool> ExcluirAlunoAsync(int id)
    {
        await _context.Alunos
            .Where(x => x.Id ==id)
            .ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
        return true;
    }
    
}

public interface IAlunoRepository
{
    Task<bool> CriarAlunoAsync(Aluno aluno);
    Task<List<Aluno>> GetAllAlunos();
    Task<bool> AtualizarAlunoAsync(Aluno aluno);
    Task<bool> ExcluirAlunoAsync(int id);
    Task<Aluno> BuscarPorIdAsync(int id);
}

