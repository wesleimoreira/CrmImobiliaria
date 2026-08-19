using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Domain.Tests
{
    public class ClienteTests
    {
        [Fact]
        public void Criar_SemTipos_Falha()
        {
            var resultado = Cliente.Criar(
                "João Silva", Fixtures.CriarTelefone(), Fixtures.CriarEmail(),
                tipos: [], corretorResponsavelId: Guid.NewGuid(), origem: OrigemCliente.Indicacao);

            Assert.False(resultado.IsSuccess);
            Assert.Contains("tipo", resultado.Error);
        }

        [Fact]
        public void Criar_OrigemCampanhaExternaSemCampanha_Falha()
        {
            var resultado = Cliente.Criar(
                "João Silva", Fixtures.CriarTelefone(), Fixtures.CriarEmail(),
                tipos: [TipoCliente.Comprador], corretorResponsavelId: Guid.NewGuid(),
                origem: OrigemCliente.CampanhaExterna, campanhaEspecifica: null);

            Assert.False(resultado.IsSuccess);
            Assert.Contains("campanha", resultado.Error, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Criar_OrigemCampanhaExternaComCampanha_Sucesso()
        {
            var resultado = Cliente.Criar(
                "João Silva", Fixtures.CriarTelefone(), Fixtures.CriarEmail(),
                tipos: [TipoCliente.Comprador], corretorResponsavelId: Guid.NewGuid(),
                origem: OrigemCliente.CampanhaExterna, campanhaEspecifica: "Feirão de Imóveis 2026");

            Assert.True(resultado.IsSuccess);
        }

        [Fact]
        public void RemoverTipo_UltimoTipo_Falha()
        {
            var cliente = Cliente.Criar(
                "João Silva", Fixtures.CriarTelefone(), Fixtures.CriarEmail(),
                tipos: [TipoCliente.Comprador], corretorResponsavelId: Guid.NewGuid(),
                origem: OrigemCliente.Indicacao).Value!;

            var resultado = cliente.RemoverTipo(TipoCliente.Comprador);

            Assert.False(resultado.IsSuccess);
        }
    }
}
