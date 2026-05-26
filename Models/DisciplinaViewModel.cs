using System.ComponentModel.DataAnnotations;

namespace Academico.Models;

public class DisciplinaViewModel
{
    public int Id { get; set; }
    public int CodigoDisciplina { get; set; }
    public string CodigoDiario { get; set; }

    [Required(ErrorMessage = "O período é obrigatório")]
    public string Periodo { get; set; }

    [Required(ErrorMessage = "O nome da disciplina é obrigatório")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O grau (Graduação/Pós) é obrigatório")]
    public string Grau { get; set; }

    [Required(ErrorMessage = "A carga horária é obrigatória")]
    public int CargaHoraria { get; set; }

    [Required(ErrorMessage = "O total de aulas é obrigatório")]
    public int TotalAulas { get; set; }

    [Required(ErrorMessage = "O turno é obrigatório")]
    public string Turno { get; set; }

    [Required(ErrorMessage = "A quantidade de etapas é obrigatória")]
    public int QuantidadeEtapas { get; set; }

    [Required(ErrorMessage = "O horário é obrigatório")]
    public string Horario { get; set; }

    [Required(ErrorMessage = "O local da aula é obrigatório")]
    public string LocalAula { get; set; }

    [Required(ErrorMessage = "Selecione um professor")]
    public int ProfessorId { get; set; }
}