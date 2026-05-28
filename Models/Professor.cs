namespace Academico.Models;
public class Professor : Pessoa
{
    public string Siape {get; set; }
    public string Area {get; set; }

    public string? ResetSenhaToken { get; set; }
    public DateTime? ResetSenhaTokenExpiraEm { get; set; }
}