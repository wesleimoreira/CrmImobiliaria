using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Domain.Tests
{
    public class AnuncioImovelTests
    {
        [Fact]
        public void Criar_TemporadaSemRegraEstadia_Falha()
        {
            var resultado = AnuncioImovel.Criar(
                Guid.NewGuid(), TipoNegociacaoImovel.Temporada,
                Fixtures.CriarCodigo(TipoNegociacaoImovel.Temporada), Fixtures.CriarDinheiro(300),
                regraEstadia: null);

            Assert.False(resultado.IsSuccess);
        }

        [Fact]
        public void Criar_VendaComRegraEstadia_Falha()
        {
            var regraEstadia = CrmImobiliaria.Domain.ValueObjects.RegraEstadia.Criar(2).Value!;

            var resultado = AnuncioImovel.Criar(
                Guid.NewGuid(), TipoNegociacaoImovel.Venda,
                Fixtures.CriarCodigo(TipoNegociacaoImovel.Venda), Fixtures.CriarDinheiro(300_000),
                regraEstadia: regraEstadia);

            Assert.False(resultado.IsSuccess);
        }

        [Fact]
        public void FluxoCompleto_DisponibilizarReservarNegociarFechar_TerminaVendido()
        {
            var anuncio = AnuncioImovel.Criar(
                Guid.NewGuid(), TipoNegociacaoImovel.Venda,
                Fixtures.CriarCodigo(TipoNegociacaoImovel.Venda), Fixtures.CriarDinheiro(300_000)).Value!;

            Assert.True(anuncio.Disponibilizar().IsSuccess);
            Assert.True(anuncio.Reservar().IsSuccess);
            Assert.True(anuncio.IniciarNegociacao().IsSuccess);
            Assert.True(anuncio.Fechar(DateTime.UtcNow).IsSuccess);

            Assert.Equal(StatusAnuncio.Vendido, anuncio.Status);
            Assert.False(anuncio.EstaAtivo);
        }

        [Fact]
        public void Fechar_ForaDeNegociacao_Falha()
        {
            var anuncio = AnuncioImovel.Criar(
                Guid.NewGuid(), TipoNegociacaoImovel.Venda,
                Fixtures.CriarCodigo(TipoNegociacaoImovel.Venda), Fixtures.CriarDinheiro(300_000)).Value!;

            var resultado = anuncio.Fechar(DateTime.UtcNow);

            Assert.False(resultado.IsSuccess);
        }

        [Fact]
        public void Fechar_AnuncioDeTemporada_NuncaFecha()
        {
            var regraEstadia = CrmImobiliaria.Domain.ValueObjects.RegraEstadia.Criar(2).Value!;
            var anuncio = AnuncioImovel.Criar(
                Guid.NewGuid(), TipoNegociacaoImovel.Temporada,
                Fixtures.CriarCodigo(TipoNegociacaoImovel.Temporada), Fixtures.CriarDinheiro(300),
                regraEstadia: regraEstadia).Value!;

            anuncio.Disponibilizar();
            anuncio.Reservar();
            anuncio.IniciarNegociacao();
            var resultado = anuncio.Fechar(DateTime.UtcNow);

            Assert.False(resultado.IsSuccess);
        }
    }
}
