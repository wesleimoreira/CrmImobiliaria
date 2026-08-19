using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed class Orcamento : Entity
    {
        public Guid PrestadorId { get; private set; }
        public Dinheiro Valor { get; private set; }
        public string? Descricao { get; private set; }
        public StatusOrcamento Status { get; private set; }

        private Orcamento(Guid prestadorId, Dinheiro valor, string? descricao)
        {
            PrestadorId = prestadorId;
            Valor = valor;
            Descricao = descricao;
            Status = StatusOrcamento.Pendente;
        }

        private Orcamento() { Valor = null!; }

        public static Result<Orcamento> Criar(Guid prestadorId, Dinheiro valor, string? descricao = null)
        {
            if (valor.Valor <= 0)
                return Result<Orcamento>.Failure("Valor do orçamento deve ser maior que zero.");

            return Result<Orcamento>.Success(new Orcamento(prestadorId, valor, descricao));
        }

        // internal: só a SolicitacaoManutencao (mesmo projeto) pode mudar o status —
        // ninguém aprova um orçamento avulso, sempre no contexto da solicitação.
        internal void Aprovar() => Status = StatusOrcamento.Aprovado;
        internal void Reprovar() => Status = StatusOrcamento.Reprovado;
    }
}
