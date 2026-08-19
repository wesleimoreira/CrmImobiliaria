using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Domain.Events;

namespace CrmImobiliaria.Domain.Tests
{
    public class VendaTests
    {
        [Fact]
        public void DeImovel_FinanciamentoBancarioSemDadosFinanciamento_Falha()
        {
            var resultado = Venda.DeImovel(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(), Fixtures.CriarDinheiro(500_000), DateOnly.FromDateTime(DateTime.Today),
                FormaPagamento.FinanciamentoBancario, financiamento: null);

            Assert.False(resultado.IsSuccess);
            Assert.Contains("financiamento", resultado.Error, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void DeImovel_AVistaComDadosFinanciamento_Falha()
        {
            var financiamento = DadosFinanciamento.Criar("Banco XYZ", Fixtures.CriarDinheiro(400_000)).Value!;

            var resultado = Venda.DeImovel(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(), Fixtures.CriarDinheiro(500_000), DateOnly.FromDateTime(DateTime.Today),
                FormaPagamento.AVista, financiamento);

            Assert.False(resultado.IsSuccess);
        }

        [Fact]
        public void DeImovel_Valido_RegistraVendaConcluidaEventComOsIdsCorretos()
        {
            var imovelId = Guid.NewGuid();
            var anuncioImovelId = Guid.NewGuid();
            var corretorId = Guid.NewGuid();
            var clienteId = Guid.NewGuid();
            var valorFinal = Fixtures.CriarDinheiro(500_000);

            var venda = Venda.DeImovel(
                Guid.NewGuid(), clienteId, Guid.NewGuid(), imovelId, anuncioImovelId,
                corretorId, valorFinal, DateOnly.FromDateTime(DateTime.Today), FormaPagamento.AVista).Value!;

            var evento = Assert.IsType<VendaConcluidaEvent>(Assert.Single(venda.EventosDominio));
            Assert.Equal(venda.Id, evento.VendaId);
            Assert.Equal(imovelId, evento.ImovelId);
            Assert.Equal(anuncioImovelId, evento.AnuncioImovelId);
            Assert.Null(evento.LoteId);
            Assert.Equal(corretorId, evento.CorretorId);
            Assert.Equal(clienteId, evento.ClienteId);
            Assert.Equal(valorFinal, evento.ValorFinal);
        }

        [Fact]
        public void DeLote_Valido_RegistraVendaConcluidaEventComLoteIdEImovelIdNulo()
        {
            var loteId = Guid.NewGuid();

            var venda = Venda.DeLote(
                Guid.NewGuid(), Guid.NewGuid(), loteId,
                Guid.NewGuid(), Fixtures.CriarDinheiro(150_000), DateOnly.FromDateTime(DateTime.Today),
                FormaPagamento.AVista).Value!;

            var evento = Assert.IsType<VendaConcluidaEvent>(Assert.Single(venda.EventosDominio));
            Assert.Equal(loteId, evento.LoteId);
            Assert.Null(evento.ImovelId);
            Assert.Null(evento.AnuncioImovelId);
        }

        [Fact]
        public void Distratar_ComMotivo_RegistraVendaDistratadaEvent()
        {
            var venda = Venda.DeLote(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(), Fixtures.CriarDinheiro(150_000), DateOnly.FromDateTime(DateTime.Today),
                FormaPagamento.AVista).Value!;

            var resultado = venda.Distratar("Comprador desistiu");

            Assert.True(resultado.IsSuccess);
            Assert.Equal(StatusVenda.Distratada, venda.Status);
            Assert.Equal(2, venda.EventosDominio.Count);
            Assert.IsType<VendaDistratadaEvent>(venda.EventosDominio[1]);
        }

        [Fact]
        public void Distratar_DuasVezes_SegundaFalha()
        {
            var venda = Venda.DeLote(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(), Fixtures.CriarDinheiro(150_000), DateOnly.FromDateTime(DateTime.Today),
                FormaPagamento.AVista).Value!;

            venda.Distratar("Motivo qualquer");
            var resultado = venda.Distratar("Outro motivo");

            Assert.False(resultado.IsSuccess);
        }
    }
}
