using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Domain.Events;
using CrmImobiliaria.Domain.ValueObjects;

namespace CrmImobiliaria.Domain.Tests
{
    public class LocacaoTests
    {
        private static Locacao NovaLocacaoAtiva(out Guid anuncioImovelId)
        {
            anuncioImovelId = Guid.NewGuid();
            var locacao = Locacao.Iniciar(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), anuncioImovelId, Guid.NewGuid()).Value!;

            locacao.RegistrarVisita();
            locacao.IniciarAnaliseCadastral();
            locacao.Aprovar();

            var periodo = PeriodoContrato.Criar(
                DateOnly.FromDateTime(DateTime.Today),
                DateOnly.FromDateTime(DateTime.Today.AddYears(1))).Value!;

            locacao.FormalizarContrato(
                Fixtures.CriarDinheiro(2000), periodo, diaVencimento: 10,
                Fixtures.CriarDinheiro(2000), Fixtures.CriarPercentual(10), IndiceReajuste.Igpm);

            locacao.RegistrarVistoriaEntrada();

            return locacao;
        }

        [Fact]
        public void EntregarChaves_AtivaLocacaoERegistraChavesEntreguesEvent()
        {
            var locacao = NovaLocacaoAtiva(out var anuncioImovelId);

            var resultado = locacao.EntregarChaves();

            Assert.True(resultado.IsSuccess);
            Assert.Equal(StatusLocacao.Ativa, locacao.Status);
            var evento = Assert.IsType<LocacaoChavesEntreguesEvent>(Assert.Single(locacao.EventosDominio));
            Assert.Equal(locacao.Id, evento.LocacaoId);
            Assert.Equal(anuncioImovelId, evento.AnuncioImovelId);
        }

        [Fact]
        public void Encerrar_LocacaoAtiva_RegistraLocacaoEncerradaEvent()
        {
            var locacao = NovaLocacaoAtiva(out var anuncioImovelId);
            locacao.EntregarChaves();

            var resultado = locacao.Encerrar();

            Assert.True(resultado.IsSuccess);
            Assert.Equal(StatusLocacao.Encerrada, locacao.Status);
            Assert.Equal(2, locacao.EventosDominio.Count);
            var evento = Assert.IsType<LocacaoEncerradaEvent>(locacao.EventosDominio[1]);
            Assert.Equal(anuncioImovelId, evento.AnuncioImovelId);
        }

        [Fact]
        public void Cancelar_AntesDeAtiva_RegistraLocacaoCanceladaEvent()
        {
            var anuncioImovelId = Guid.NewGuid();
            var locacao = Locacao.Iniciar(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), anuncioImovelId, Guid.NewGuid()).Value!;

            var resultado = locacao.Cancelar("Locatário desistiu");

            Assert.True(resultado.IsSuccess);
            Assert.Equal(StatusLocacao.Cancelada, locacao.Status);
            var evento = Assert.IsType<LocacaoCanceladaEvent>(Assert.Single(locacao.EventosDominio));
            Assert.Equal(anuncioImovelId, evento.AnuncioImovelId);
        }

        [Fact]
        public void Encerrar_LocacaoNaoAtiva_Falha()
        {
            var locacao = Locacao.Iniciar(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Value!;

            var resultado = locacao.Encerrar();

            Assert.False(resultado.IsSuccess);
        }
    }
}
