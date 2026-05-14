using Academico.Models;
using Microsoft.EntityFrameworkCore;

namespace Academico.Repositories;

public class ProfessorRepository : IProfessorRepository
{
    readonly AcademicoContext _context;

    public ProfessorRepository(AcademicoContext context)
    {
        _context = context;
    }

    public async Task<List<Professor>> GetAllProfessores()
    {
        return await _context.Professores.ToListAsync();
    }

    public async Task<Professor> BuscarPorIdAsync(int id)
    {
        return await _context.Professores.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> CriarProfessorAsync(Professor professor)
    {
        await _context.AddAsync(professor);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> AtualizarProfessorAsync(Professor professor)
    {
        var professorBanco = await _context.Professores.FirstOrDefaultAsync(x => x.Id == professor.Id);
        professorBanco!.Nome = professor.Nome;
        professorBanco.Cpf = professor.Cpf;
        professorBanco.Area = professor.Area;
        professorBanco.DataNascimento = professor.DataNascimento;
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> ExcluirProfessorAsync(int id)
    {
        await _context.Professores
            .Where(x => x.Id ==id)
            .ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
        return true;
    }
}

public interface IProfessorRepository
{
    Task<List<Professor>> GetAllProfessores();
    Task <bool> CriarProfessorAsync(Professor Professor);
    Task<bool> AtualizarProfessorAsync(Professor professor);
    Task<bool> ExcluirProfessorAsync(int id);
    Task<Professor> BuscarPorIdAsync(int id);
}