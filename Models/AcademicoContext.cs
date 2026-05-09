using Microsoft.EntityFrameworkCore;

namespace Academico.Models;

public class AcademicoContext : DbContext
{
    public AcademicoContext(DbContextOptions options)
    : base(options)
    {
    }
    public DbSet<Professor> Professores { get; set; }
    public DbSet<Aluno> Alunos {get; set; }
}