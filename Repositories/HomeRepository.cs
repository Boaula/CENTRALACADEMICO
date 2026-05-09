using Academico.Models;
using Microsoft.EntityFrameworkCore;

namespace Academico.Repositories;

public class HomeRepository : IHomeRepository
{
    readonly AcademicoContext _context;

    public HomeRepository(AcademicoContext context)
    {
        _context = context;
    }

    public async Task<List<Aluno>> GetAllAlunos()
    {
        return await _context.Alunos.ToListAsync();
    }

    public async Task<List<Professor>> GetAllProfessores()
    {
        return await _context.Professores.ToListAsync();
    }
}
public interface IHomeRepository
{
    Task<List<Aluno>> GetAllAlunos();
    Task<List<Professor>> GetAllProfessores();
}