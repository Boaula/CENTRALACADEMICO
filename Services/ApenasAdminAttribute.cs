using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Academico.Services;

// Esse atributo pode ser colocado em cima de classes (Controllers) ou métodos (Actions)
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApenasAdminAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var usuario = context.HttpContext.User;

        // 1. Se não estiver logado, manda para a tela de login do ADM
        if (usuario.Identity == null || !usuario.Identity.IsAuthenticated)
        {
            context.Result = new RedirectToActionResult("Entrar", "Login", null);
            return;
        }

        // 2. Se tiver a Claim "Matricula", significa que é um ALUNO tentando invadir!
        if (usuario.HasClaim(c => c.Type == "Matricula"))
        {
            // Redireciona o aluno para a página de acesso negado do ADM
            context.Result = new RedirectToActionResult("AcessoNegado", "Login", null);
        }
    }
}