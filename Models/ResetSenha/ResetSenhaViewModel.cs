namespace Academico.Models.ResetSenha

{

    public class ResetSenhaViewModel
    {
        public string Token { get; set; } = string.Empty;
        public string NovaSenha { get; set; } = string.Empty;
        public string ConfirmarSenha { get; set; } = string.Empty;
    }

}
