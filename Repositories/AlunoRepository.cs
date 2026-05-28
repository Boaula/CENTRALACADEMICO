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
        // 1. Busca o aluno original direto do banco usando o ID
        var alunoBanco = await _context.Alunos.FindAsync(aluno.Id);
        
        if (alunoBanco == null) return false;

        // 2. Atualiza apenas o que o ADM pode mexer na tela
        alunoBanco.Nome = aluno.Nome;
        alunoBanco.Email = aluno.Email;
        alunoBanco.DataNascimento = aluno.DataNascimento;
        alunoBanco.Curso = aluno.Curso;

        // 3. Atualiza as chaves tratadas que o ADM editou
        alunoBanco.Cpf = aluno.Cpf;
        alunoBanco.UserName = aluno.UserName;

        //NOTA: Repare que NÃO alteramos o alunoBanco.Matricula aqui!
        // Ele manterá a matrícula original que já estava salva no MySQL.

        return await _context.SaveChangesAsync() > 0;
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

