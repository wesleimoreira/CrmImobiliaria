using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed class Repasse : AggregateRoot
    {
        private readonly List<DespesaAutorizada> _despesas = [];

        public Guid LocacaoId { get; private set; }
        public Guid PagamentoAluguelId { get; private set; }
        public Competencia Competencia { get; private set; }
        public Dinheiro ValorAluguelRecebido { get; private set; }
        public Percentual TaxaAdministracao { get; private set; }
        public Dinheiro ValorTaxaAdministracao { get; private set; }
        public IReadOnlyList<DespesaAutorizada> Despesas => _despesas;
        public Dinheiro ValorLiquido { get; private set; }
        public StatusRepasse Status { get; private set; }
        public DateOnly? DataRepasse { get; private set; }
        public string? Comprovante { get; private set; }

        private Repasse(Guid locacaoId, Guid pagamentoAluguelId, Competencia competencia, Dinheiro valorAluguelRecebido, Percentual taxaAdministracao, Dinheiro valorTaxaAdministracao)
        {
            LocacaoId = locacaoId;
            PagamentoAluguelId = pagamentoAluguelId;
            Competencia = competencia;
            ValorAluguelRecebido = valorAluguelRecebido;
            TaxaAdministracao = taxaAdministracao;
            ValorTaxaAdministracao = valorTaxaAdministracao;
            Status = StatusRepasse.Pendente;
            RecalcularValorLiquido();
        }

        private Repasse()
        {
            Competencia = null!;
            ValorAluguelRecebido = null!;
            TaxaAdministracao = null!;
            ValorTaxaAdministracao = null!;
            ValorLiquido = null!;
        }

        public static Result<Repasse> Calcular(Guid locacaoId, Guid pagamentoAluguelId, Competencia competencia, Dinheiro valorAluguelRecebido, Percentual taxaAdministracao)
        {
            var valorTaxa = valorAluguelRecebido * taxaAdministracao;
            return Result<Repasse>.Success(new Repasse(locacaoId, pagamentoAluguelId, competencia, valorAluguelRecebido, taxaAdministracao, valorTaxa));
        }

        public Result<bool> AdicionarDespesa(string descricao, Dinheiro valor)
        {
            if (Status == StatusRepasse.Realizado)
                return Result<bool>.Failure("Não é possível adicionar despesa a um repasse já realizado.");

            if (string.IsNullOrWhiteSpace(descricao))
                return Result<bool>.Failure("Descrição da despesa é obrigatória.");

            _despesas.Add(new DespesaAutorizada(descricao.Trim(), valor));
            RecalcularValorLiquido();
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        private void RecalcularValorLiquido()
        {
            var somaDespesas = _despesas.Aggregate(Dinheiro.Zero, (acc, d) => acc + d.Valor);
            ValorLiquido = ValorAluguelRecebido - ValorTaxaAdministracao - somaDespesas;
        }

        public Result<bool> Confirmar(DateOnly dataRepasse, string comprovante)
        {
            if (Status == StatusRepasse.Realizado)
                return Result<bool>.Failure("Repasse já confirmado.");

            if (string.IsNullOrWhiteSpace(comprovante))
                return Result<bool>.Failure("Comprovante é obrigatório para confirmar o repasse.");

            Status = StatusRepasse.Realizado;
            DataRepasse = dataRepasse;
            Comprovante = comprovante.Trim();
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }
    }
}
