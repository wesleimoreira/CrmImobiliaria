using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Domain.Tests
{
    public class RegraComissaoTests
    {
        private static RegraComissao NovaRegra() =>
            RegraComissao.Criar("Padrão", Fixtures.CriarPercentual(6), empreendimentoId: Guid.NewGuid()).Value!;

        [Fact]
        public void Criar_ComEmpreendimentoEImovel_Falha()
        {
            var resultado = RegraComissao.Criar(
                "Regra inválida", Fixtures.CriarPercentual(6),
                empreendimentoId: Guid.NewGuid(), imovelId: Guid.NewGuid());

            Assert.False(resultado.IsSuccess);
        }

        [Fact]
        public void DefinirRateio_SomaDiferenteDeCem_Falha()
        {
            var regra = NovaRegra();

            var resultado = regra.DefinirRateio([
                new ItemRateio(PapelComissao.Imobiliaria, Fixtures.CriarPercentual(50)),
                new ItemRateio(PapelComissao.Captador, Fixtures.CriarPercentual(30))
            ]);

            Assert.False(resultado.IsSuccess);
            Assert.Empty(regra.Rateio);
        }

        [Fact]
        public void DefinirRateio_PapelRepetido_Falha()
        {
            var regra = NovaRegra();

            var resultado = regra.DefinirRateio([
                new ItemRateio(PapelComissao.Imobiliaria, Fixtures.CriarPercentual(50)),
                new ItemRateio(PapelComissao.Imobiliaria, Fixtures.CriarPercentual(50))
            ]);

            Assert.False(resultado.IsSuccess);
        }

        [Fact]
        public void DefinirRateio_SomaCem_Sucesso()
        {
            var regra = NovaRegra();

            var resultado = regra.DefinirRateio([
                new ItemRateio(PapelComissao.Imobiliaria, Fixtures.CriarPercentual(40)),
                new ItemRateio(PapelComissao.Captador, Fixtures.CriarPercentual(30)),
                new ItemRateio(PapelComissao.Negociacao, Fixtures.CriarPercentual(30))
            ]);

            Assert.True(resultado.IsSuccess);
            Assert.Equal(3, regra.Rateio.Count);
        }
    }
}
