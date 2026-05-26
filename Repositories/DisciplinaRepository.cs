using Academico.Models;
using Academico.Services;
using Microsoft.EntityFrameworkCore;

namespace Academico.Repositories;

public class DisciplinaRepository : IDisciplinaRepository
{
    private readonly AcademicoContext _context; 
    private readonly GeradorCodigoService _geradorCodigo;
    private static readonly Random _random = new Random(); // Instância única para evitar sementes repetidas

    public DisciplinaRepository(AcademicoContext context, GeradorCodigoService geradorCodigo)
    {
        _context = context;
        _geradorCodigo = geradorCodigo;
    }

    // 1. O método que salva a disciplina no banco (Chamado pelo POST do Controller)
public async Task<bool> AdicionarAsync(Disciplina disciplina)
    {
try
        {
            // O repositório delega a responsabilidade de gerar o código para o serviço
            disciplina.CodigoDiario = await _geradorCodigo.GerarCodigoDiarioUnicoAsync();

            // 2. Gera o Código da Disciplina Geral (Ex: 53469)
            disciplina.CodigoDisciplina = await _geradorCodigo.GerarCodigoDisciplinaUnicoAsync();

            await _context.Disciplinas.AddAsync(disciplina);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

// 1. IMPLEMENTAÇÃO DO ATUALIZAR ASYNC
    public async Task<bool> AtualizarAsync(Disciplina disciplina)
    {
        try
        {
            _context.Disciplinas.Update(disciplina);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            // Caso ocorra algum erro de banco, ele não quebra a aplicação
            return false;
        }
    }

    // 2. IMPLEMENTAÇÃO DO EXCLUIR ASYNC
    public async Task<bool> ExcluirAsync(int id)
    {
        try
        {
            var disciplina = await _context.Disciplinas.FindAsync(id);
            if (disciplina == null) return false;

            _context.Disciplinas.Remove(disciplina);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    // 2. Método bônus para listar todas as disciplinas
    public async Task<List<Disciplina>> GetAllDisciplinasAsync()
    {
        // O .Include faz o "JOIN" no banco para trazer os dados do Professor junto
        return await _context.Disciplinas
            .Include(d => d.Professor) 
            .ToListAsync();
    }

    // 3. Método bônus para buscar uma única disciplina por ID
    public async Task<Disciplina> BuscarPorIdAsync(int id)
    {
        return await _context.Disciplinas
            .Include(d => d.Professor)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}