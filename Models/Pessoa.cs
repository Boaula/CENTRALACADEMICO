namespace Academico.Models;

public class Pessoa
{
    public int Id {get; set; }
    public string Nome {get; set; }
    public string Cpf {get; set; }
    public DateOnly DataNascimento {get; set; }
}