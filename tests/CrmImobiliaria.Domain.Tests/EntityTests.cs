using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Domain.Tests
{
    public class EntityTests
    {
        [Fact]
        public void Criar_DefineCriadoEmComDataAtual()
        {
            var antes = DateTime.UtcNow;

            var lead = Lead.Criar(Guid.NewGuid(), Guid.NewGuid()).Value!;

            var depois = DateTime.UtcNow;

            Assert.InRange(lead.CriadoEm, antes, depois);
        }
    }
}
