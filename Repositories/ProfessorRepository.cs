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
        // Busca o registro atual do banco para evitar conflito de tracking
        var professorBanco = await _context.Professores.FindAsync(professor.Id);
        
        if (professorBanco == null) return false;

        // Atualiza os campos manualmente
        professorBanco.Nome = professor.Nome;
        professorBanco.Email = professor.Email;
        professorBanco.DataNascimento = professor.DataNascimento;
        professorBanco.Area = professor.Area;
        
        // Atualiza o CPF e o UserName com os novos valores enviados pelo ADM
        professorBanco.Cpf = professor.Cpf;
        professorBanco.UserName = professor.UserName; 

        // Salva as alterações
        return await _context.SaveChangesAsync() > 0;
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