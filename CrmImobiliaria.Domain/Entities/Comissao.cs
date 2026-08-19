using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed class Comissao : AggregateRoot
    {
        private readonly List<RateioComissao> _rateio = [];

        public OrigemComissao OrigemTipo { get; private set; }
        public Guid OrigemId { get; private set; }           // VendaId ou LocacaoId, conforme OrigemTipo
        public Guid RegraComissaoId { get; private set; }
        public Dinheiro ValorBase { get; private set; }       // valor da venda ou do primeiro aluguel
        public Percentual PercentualComissao { get; private set; }
        public Dinheiro ComissaoTotal { get; private set; }   // cobre "Comissão total" e "Comissão prevista" do documento — mesmo valor
        public Dinheiro ComissaoRecebida { get; private set; }
        public Dinheiro ComissaoDistribuida { get; private set; }
        public IReadOnlyList<RateioComissao> Rateio => _rateio;

        public Dinheiro Saldo => ComissaoRecebida - ComissaoDistribuida;

        private Comissao(OrigemComissao origemTipo, Guid origemId, Guid regraComissaoId, Dinheiro valorBase, Percentual percentualComissao, Dinheiro comissaoTotal, List<RateioComissao> rateio)
        {
            OrigemTipo = origemTipo;
            OrigemId = origemId;
            RegraComissaoId = regraComissaoId;
            ValorBase = valorBase;
            PercentualComissao = percentualComissao;
            ComissaoTotal = comissaoTotal;
            ComissaoRecebida = Dinheiro.Zero;
            ComissaoDistribuida = Dinheiro.Zero;
            _rateio = rateio;
        }

        private Comissao()
        {
            ValorBase = null!;
            PercentualComissao = null!;
            ComissaoTotal = null!;
            ComissaoRecebida = null!;
            ComissaoDistribuida = null!;
        }

        // regra já validada pelo Application (rateio fechado em 100%) antes de chegar aqui
        public static Result<Comissao> Gerar(OrigemComissao origemTipo, Guid origemId, RegraComissao regra, Dinheiro valorBase, IReadOnlyDictionary<PapelComissao, Guid?>? beneficiarios = null)
        {
            if (regra.Rateio.Count == 0)
                return Result<Comissao>.Failure("A regra de comissão não tem rateio definido.");

            var comissaoTotal = valorBase * regra.PercentualComissaoTotal;

            var rateioCalculado = regra.Rateio
                .Select(item => new RateioComissao(
                    item.Papel,
                    item.Percentual,
                    comissaoTotal * item.Percentual,
                    beneficiarios?.GetValueOrDefault(item.Papel)))
                .ToList();

            return Result<Comissao>.Success(new Comissao(origemTipo, origemId, regra.Id, valorBase, regra.PercentualComissaoTotal, comissaoTotal, rateioCalculado));
        }

        public Result<bool> RegistrarRecebimento(Dinheiro valor)
        {
            if (valor.Valor <= 0)
                return Result<bool>.Failure("Valor do recebimento deve ser maior que zero.");

            var novoTotal = ComissaoRecebida + valor;

            if (novoTotal.Valor > ComissaoTotal.Valor)
                return Result<bool>.Failure("Valor recebido não pode ultrapassar a comissão total.");

            ComissaoRecebida = novoTotal;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> RegistrarDistribuicao(Dinheiro valor)
        {
            if (valor.Valor <= 0)
                return Result<bool>.Failure("Valor da distribuição deve ser maior que zero.");

            var novoTotal = ComissaoDistribuida + valor;
            if (novoTotal.Valor > ComissaoRecebida.Valor)
                return Result<bool>.Failure("Não é possível distribuir mais do que foi recebido.");

            ComissaoDistribuida = novoTotal;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }
    }
}
