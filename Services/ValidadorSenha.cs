namespace Academico.Services;

public class ValidadorSenha
{
    public string Senha { get; private set; }
    public string ConfirmacaoSenha { get; private set; }
    private readonly List<string> _erros;

    // Propriedade que expõe os erros encontrados (Encapsulamento)
    public IReadOnlyCollection<string> Erros => _erros.AsReadOnly();
    
    // Propriedade calculada para dizer se a senha é válida ou não
    public bool EhValida => _erros.Count == 0;

    public ValidadorSenha(string senha, string confirmacaoSenha)
    {
        Senha = senha ?? string.Empty;
        ConfirmacaoSenha = confirmacaoSenha ?? string.Empty;
        _erros = new List<string>();
        
        Validar();
    }

    // Regras de negócio encapsuladas no objeto
    private void Validar()
    {
        if (Senha != ConfirmacaoSenha)
        {
            _erros.Add("A senha e a confirmação de senha não coincidem.");
            return; // Se não coincidem, não precisa validar o resto
        }

        if (Senha.Length < 6)
        {
            _erros.Add("A senha deve ter no mínimo 6 caracteres.");
        }

        if (!Senha.Any(char.IsDigit))
        {
            _erros.Add("A senha deve conter pelo menos um número.");
        }

        if (!Senha.Any(char.IsLetter))
        {
            _erros.Add("A senha deve conter pelo menos uma letra.");
        }
    }
}