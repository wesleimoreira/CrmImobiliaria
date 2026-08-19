using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Domain.Tests
{
    public class LoteTests
    {
        private static Lote NovoLote() => Lote.Criar(
            Guid.NewGuid(), "Q1", "10", Fixtures.CriarArea(300),
            Fixtures.CriarDinheiro(100_000), Fixtures.CriarDinheiro(10_000), parcelamentoMaximo: 60).Value!;

        [Fact]
        public void Vender_LoteDisponivel_Falha()
        {
            var lote = NovoLote();

            var resultado = lote.Vender();

            Assert.False(resultado.IsSuccess);
            Assert.Equal(StatusLote.Disponivel, lote.Status);
        }

        [Fact]
        public void Vender_LoteReservadoSemProposta_Falha()
        {
            var lote = NovoLote();
            lote.Reservar();

            var resultado = lote.Vender();

            Assert.False(resultado.IsSuccess);
            Assert.Equal(StatusLote.Reservado, lote.Status);
        }

        [Fact]
        public void Vender_LoteEmProposta_Sucesso_ImpedeVendaDuplicada()
        {
            var lote = NovoLote();
            lote.Reservar();
            lote.IniciarProposta();

            var primeiraVenda = lote.Vender();
            var segundaVenda = lote.Vender();

            Assert.True(primeiraVenda.IsSuccess);
            Assert.Equal(StatusLote.Vendido, lote.Status);
            Assert.False(segundaVenda.IsSuccess);
        }
    }
}
