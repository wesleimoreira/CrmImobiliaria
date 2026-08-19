using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed class PerfilInteresse : Entity
    {
        public TipoNegociacaoImovel TipoNegociacao { get; private set; }   // Venda ou Locação (Temporada não se aplica aqui)
        public TipoImovel TipoImovel { get; private set; }
        public string? LocalizacaoDesejada { get; private set; }
        public FaixaDeValor FaixaPreco { get; private set; }
        public int? NumeroQuartos { get; private set; }
        public FormaPagamento FormaPagamento { get; private set; }
        public string? Observacoes { get; private set; }

        private PerfilInteresse(TipoNegociacaoImovel tipoNegociacao, TipoImovel tipoImovel, FaixaDeValor faixaPreco, string? localizacaoDesejada, int? numeroQuartos, FormaPagamento formaPagamento, string? observacoes)
        {
            TipoNegociacao = tipoNegociacao;
            TipoImovel = tipoImovel;
            FaixaPreco = faixaPreco;
            LocalizacaoDesejada = localizacaoDesejada;
            NumeroQuartos = numeroQuartos;
            FormaPagamento = formaPagamento;
            Observacoes = observacoes;
        }

        private PerfilInteresse()
        {
            FaixaPreco = null!;
        }

        public static Result<PerfilInteresse> Criar(TipoNegociacaoImovel tipoNegociacao, TipoImovel tipoImovel, FaixaDeValor faixaPreco, string? localizacaoDesejada = null, int? numeroQuartos = null, FormaPagamento formaPagamento = FormaPagamento.AVista, string? observacoes = null)
        {
            if (tipoNegociacao == TipoNegociacaoImovel.Temporada)
                return Result<PerfilInteresse>.Failure("Perfil de interesse não se aplica a temporada.");

            if (numeroQuartos is < 0)
                return Result<PerfilInteresse>.Failure("Número de quartos não pode ser negativo.");

            return Result<PerfilInteresse>.Success(new PerfilInteresse(
                tipoNegociacao, tipoImovel, faixaPreco, localizacaoDesejada, numeroQuartos, formaPagamento, observacoes));
        }

        public void Atualizar(FaixaDeValor faixaPreco, string? localizacaoDesejada, int? numeroQuartos, FormaPagamento formaPagamento, string? observacoes)
        {
            FaixaPreco = faixaPreco;
            LocalizacaoDesejada = localizacaoDesejada;
            NumeroQuartos = numeroQuartos;
            FormaPagamento = formaPagamento;
            Observacoes = observacoes;
            MarcarAtualizado();
        }

        // Usado pelo módulo de cruzamento automático cliente x imóvel (mencionado no seu documento)
        public bool Compativel(Dinheiro valorAnuncio, TipoImovel tipoImovelAnuncio, TipoNegociacaoImovel tipoNegociacaoAnuncio) => tipoNegociacaoAnuncio == TipoNegociacao && tipoImovelAnuncio == TipoImovel && FaixaPreco.Contem(valorAnuncio);
    }
}
