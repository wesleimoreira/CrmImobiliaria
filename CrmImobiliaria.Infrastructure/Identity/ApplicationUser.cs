using Microsoft.AspNetCore.Identity;

namespace CrmImobiliaria.Infrastructure.Identity
{
    public sealed class ApplicationUser : IdentityUser<Guid>
    {
        public string NomeCompleto { get; set; } = "";
        public Guid? CorretorId { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
