using System;
using System.ComponentModel.DataAnnotations;

namespace Academico.Models;

public class CadastroAlunoViewModel
{
    [Required(ErrorMessage = "O Nome é obrigatório.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O E-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O e-mail não é válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O CPF é obrigatório.")]
    public string Cpf { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
    public DateOnly DataNascimento { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [DataType(DataType.Password)]
    public string Senha { get; set; } = string.Empty;

    // A PROPRIEDADE QUE ESTAVA FALTANDO E GEROU OS 4 ERROS:
    [Required(ErrorMessage = "A confirmação de senha é obrigatória.")]
    [DataType(DataType.Password)]
    public string ConfirmarSenha { get; set; } = string.Empty;
}