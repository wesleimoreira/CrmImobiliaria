using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Domain.Tests
{
    public class LeadTests
    {
        private static Lead NovoLead() => Lead.Criar(Guid.NewGuid(), Guid.NewGuid()).Value!;

        [Fact]
        public void Criar_ComecaNoEstagioNovoLead()
        {
            var lead = NovoLead();

            Assert.Equal(EstagioFunil.NovoLead, lead.EstagioAtual);
            Assert.Single(lead.Historico);
        }

        [Fact]
        public void AvancarPara_MesmoEstagioNovamente_Falha()
        {
            var lead = NovoLead();
            lead.RegistrarContato();

            var resultado = lead.RegistrarContato();

            Assert.False(resultado.IsSuccess);
            Assert.Equal(EstagioFunil.Contato, lead.EstagioAtual);
        }

        [Fact]
        public void AvancarPara_EstagioAnterior_Falha()
        {
            var lead = NovoLead();
            lead.RegistrarProposta();

            var resultado = lead.Qualificar();

            Assert.False(resultado.IsSuccess);
            Assert.Equal(EstagioFunil.Proposta, lead.EstagioAtual);
        }

        [Fact]
        public void MarcarPerdido_SemMotivo_Falha()
        {
            var lead = NovoLead();

            var resultado = lead.MarcarPerdido("");

            Assert.False(resultado.IsSuccess);
        }

        [Fact]
        public void MarcarPerdido_DepoisReabrir_VoltaAoEstagioAnterior()
        {
            var lead = NovoLead();
            lead.RegistrarContato();
            lead.Qualificar();

            lead.MarcarPerdido("Cliente desistiu");
            Assert.Equal(EstagioFunil.Perdido, lead.EstagioAtual);

            var resultado = lead.Reabrir();

            Assert.True(resultado.IsSuccess);
            Assert.Equal(EstagioFunil.Qualificado, lead.EstagioAtual);
            Assert.Null(lead.MotivoPerda);
        }
    }
}
