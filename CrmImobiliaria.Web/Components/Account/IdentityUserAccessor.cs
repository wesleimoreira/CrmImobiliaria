using CrmImobiliaria.Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CrmImobiliaria.Web.Components.Account
{
    internal sealed class IdentityUserAccessor(UserManager<ApplicationUser> userManager, IdentityRedirectManager redirectManager)
    {
        public async Task<ApplicationUser> GetRequiredUserAsync(HttpContext context)
        {
            var user = await userManager.GetUserAsync(context.User);

            if (user is null)
                redirectManager.RedirectToWithStatus("/conta/login", "Erro: usuário não encontrado.", context);

            return user!;
        }
    }
}
