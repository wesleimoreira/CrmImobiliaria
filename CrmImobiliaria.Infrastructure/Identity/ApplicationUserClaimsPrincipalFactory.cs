using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CrmImobiliaria.Infrastructure.Identity
{
    public sealed class ApplicationUserClaimsPrincipalFactory(
        UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, IOptions<IdentityOptions> optionsAccessor)
        : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole<Guid>>(userManager, roleManager, optionsAccessor)
    {
        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);
            ((ClaimsIdentity)principal.Identity!).AddClaim(new Claim("nome_completo", user.NomeCompleto));
            return principal;
        }
    }
}
