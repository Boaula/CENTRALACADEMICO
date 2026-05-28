namespace Academico.Models;

public class Aluno : Pessoa
{
    public string Matricula {get; set; }
    public string Curso {get; set; } = string.Empty;
    public string Senha { get; set; } = "";

    public string? ResetSenhaToken { get; set; }
    public DateTime? ResetSenhaTokenExpiraEm { get; set; }

}