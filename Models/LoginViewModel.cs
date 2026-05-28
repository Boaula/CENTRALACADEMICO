using System.ComponentModel.DataAnnotations;

namespace Academico.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "O CPF é obrigatório.")]
    public string Cpf { get; set; } = string.Empty; // Usado para receber o CPF na tela

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [DataType(DataType.Password)]
    public string Senha { get; set; } = string.Empty;
}