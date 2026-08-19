using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed class AnuncioImovel : Entity
    {
        public Guid ImovelId { get; private set; }
        public TipoNegociacaoImovel Tipo { get; private set; }
        public CodigoImovel Codigo { get; private set; }
        public Dinheiro Valor { get; private set; }
        public StatusAnuncio Status { get; private set; }
        public bool Exclusividade { get; private set; }
        public RegraEstadia? RegraEstadia { get; private set; }
        public DateTime? DataFechamento { get; private set; }

        private AnuncioImovel(Guid imovelId, TipoNegociacaoImovel tipo, CodigoImovel codigo, Dinheiro valor, bool exclusividade, RegraEstadia? regraEstadia)
        {
            ImovelId = imovelId;
            Tipo = tipo;
            Codigo = codigo;
            Valor = valor;
            Exclusividade = exclusividade;
            RegraEstadia = regraEstadia;
            Status = StatusAnuncio.EmCaptacao;
        }

        private AnuncioImovel()
        {
            Codigo = null!;
            Valor = null!;
        }

        public static Result<AnuncioImovel> Criar(Guid imovelId, TipoNegociacaoImovel tipo, CodigoImovel codigo, Dinheiro valor, bool exclusividade = false, RegraEstadia? regraEstadia = null)
        {
            if (tipo == TipoNegociacaoImovel.Temporada && regraEstadia is null)
                return Result<AnuncioImovel>.Failure("Anúncio de temporada exige regra de estadia.");

            if (tipo != TipoNegociacaoImovel.Temporada && regraEstadia is not null)
                return Result<AnuncioImovel>.Failure("Regra de estadia só se aplica a anúncios de temporada.");

            if (codigo.Tipo != tipo)
                return Result<AnuncioImovel>.Failure("Tipo do código não corresponde ao tipo do anúncio.");

            return Result<AnuncioImovel>.Success(new AnuncioImovel(imovelId, tipo, codigo, valor, exclusividade, regraEstadia));
        }

        public void AtualizarValor(Dinheiro valor)
        {
            Valor = valor;
            MarcarAtualizado();
        }

        public void DefinirExclusividade(bool exclusividade)
        {
            Exclusividade = exclusividade;
            MarcarAtualizado();
        }

        public Result<bool> Disponibilizar()
        {
            if (Status is not (StatusAnuncio.EmCaptacao or StatusAnuncio.Suspenso))
                return Result<bool>.Failure($"Não é possível disponibilizar um anúncio no status {Status}.");

            Status = StatusAnuncio.Disponivel;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> Reservar()
        {
            if (Status != StatusAnuncio.Disponivel)
                return Result<bool>.Failure("Só é possível reservar um anúncio disponível.");

            Status = StatusAnuncio.Reservado;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> IniciarNegociacao()
        {
            if (Status is not (StatusAnuncio.Disponivel or StatusAnuncio.Reservado))
                return Result<bool>.Failure("Só é possível iniciar negociação a partir de Disponível ou Reservado.");

            Status = StatusAnuncio.EmNegociacao;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> Fechar(DateTime dataFechamento)
        {
            if (Tipo == TipoNegociacaoImovel.Temporada)
                return Result<bool>.Failure("Anúncio de temporada não fecha definitivamente — use o módulo de reservas.");

            if (Status != StatusAnuncio.EmNegociacao)
                return Result<bool>.Failure("Só é possível fechar um anúncio em negociação.");

            Status = Tipo == TipoNegociacaoImovel.Venda ? StatusAnuncio.Vendido : StatusAnuncio.Alugado;
            DataFechamento = dataFechamento;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> CancelarNegociacao()
        {
            if (Status is not (StatusAnuncio.Reservado or StatusAnuncio.EmNegociacao))
                return Result<bool>.Failure("Não há negociação em andamento para cancelar.");

            Status = StatusAnuncio.Disponivel;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> Suspender()
        {
            if (Status is StatusAnuncio.Vendido or StatusAnuncio.Alugado)
                return Result<bool>.Failure($"Não é possível suspender um anúncio {Status}.");

            Status = StatusAnuncio.Suspenso;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> Reabrir()
        {
            if (Status is not (StatusAnuncio.Vendido or StatusAnuncio.Alugado))
                return Result<bool>.Failure("Só é possível reabrir um anúncio vendido ou alugado.");

            Status = StatusAnuncio.Disponivel;
            DataFechamento = null;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public bool EstaAtivo => Status is not (StatusAnuncio.Vendido or StatusAnuncio.Suspenso);
    }
}
