using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed class RegraComissao : AggregateRoot
    {
        private readonly List<ItemRateio> _rateio = [];

        public string Nome { get; private set; }
        public Percentual PercentualComissaoTotal { get; private set; }
        public Guid? EmpreendimentoId { get; private set; }
        public Guid? ImovelId { get; private set; }
        public IReadOnlyList<ItemRateio> Rateio => _rateio;

        private RegraComissao(string nome, Percentual percentualComissaoTotal, Guid? empreendimentoId, Guid? imovelId)
        {
            Nome = nome;
            PercentualComissaoTotal = percentualComissaoTotal;
            EmpreendimentoId = empreendimentoId;
            ImovelId = imovelId;
        }

        private RegraComissao()
        {
            Nome = null!;
            PercentualComissaoTotal = null!;
        }

        public static Result<RegraComissao> Criar(string? nome, Percentual percentualComissaoTotal, Guid? empreendimentoId = null, Guid? imovelId = null)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return Result<RegraComissao>.Failure("Nome da regra é obrigatório.");

            if (empreendimentoId is not null && imovelId is not null)
                return Result<RegraComissao>.Failure("Vincule a um empreendimento OU a um imóvel, não aos dois.");

            return Result<RegraComissao>.Success(new RegraComissao(nome.Trim(), percentualComissaoTotal, empreendimentoId, imovelId));
        }

        public Result<bool> DefinirRateio(IEnumerable<ItemRateio> itens)
        {
            var lista = itens.ToList();

            if (lista.Select(i => i.Papel).Distinct().Count() != lista.Count)
                return Result<bool>.Failure("Cada papel só pode aparecer uma vez no rateio.");

            var soma = lista.Sum(i => i.Percentual.Valor);

            if (Math.Abs(soma - 100m) > 0.01m)
                return Result<bool>.Failure($"A soma dos percentuais deve ser 100%. Atual: {soma}%.");

            _rateio.Clear();
            _rateio.AddRange(lista);
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Dinheiro Projetar(Dinheiro valorEstimado) => valorEstimado * PercentualComissaoTotal;
    }
}
