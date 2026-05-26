namespace Academico.Models;

public class Disciplina
{
    public int Id { get; set; }
    public string Periodo { get; set; } // Ex: 2026/1
    public string CodigoDiario { get; set; } // Ex: 20261.3.20241233.131.1N
    public int CodigoDisciplina { get; set; } // Ex: 53469
    public string Nome { get; set; } // Ex: Programação WEB I
    public string Grau { get; set; } // Ex: Graduação
    public int CargaHoraria { get; set; } // Ex: 68
    public int TotalAulas { get; set; } // Ex: 80
    public string Turno { get; set; } // Ex: Noturno
    public int QuantidadeEtapas { get; set; } // Ex: 1
    public string Horario { get; set; } // Ex: 5N1234
    public string LocalAula { get; set; } // Ex: SALA AULA A2...
    
    //Relacionamento com Professor
    public int ProfessorId { get; set; }
    public Professor Professor { get; set; } // Propriedade de navegação
}