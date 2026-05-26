using Academico.Models;

namespace Academico.Repositories;

public interface IDisciplinaRepository
{
    Task<bool> AdicionarAsync(Disciplina disciplina);
    Task<List<Disciplina>> GetAllDisciplinasAsync();
    Task<Disciplina> BuscarPorIdAsync(int id);

    Task<bool> AtualizarAsync(Disciplina disciplina);
    Task<bool> ExcluirAsync(int id);
}