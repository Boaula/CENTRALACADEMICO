using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Academico.Models;

public class Pessoa : IdentityUser<int>
{
    [Required]
    public string Nome { get; set; }

    [Required]
    public string Cpf { get; set; }

    [Required]
    public DateOnly DataNascimento { get; set; }
}