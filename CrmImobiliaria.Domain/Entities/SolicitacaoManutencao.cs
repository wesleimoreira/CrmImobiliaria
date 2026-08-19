using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed class SolicitacaoManutencao : AggregateRoot
    {
        private readonly List<Orcamento> _orcamentos = [];

        public Guid ImovelId { get; private set; }
        public Guid? LocacaoId { get; private set; }
        public Guid SolicitanteId { get; private set; }   // Cliente: proprietário ou locatário
        public string Descricao { get; private set; }
        public StatusSolicitacaoManutencao Status { get; private set; }
        public Guid? OrcamentoAprovadoId { get; private set; }
        public DateTime? DataConclusao { get; private set; }
        public IReadOnlyList<Orcamento> Orcamentos => _orcamentos;

        private SolicitacaoManutencao(Guid imovelId, Guid solicitanteId, string descricao, Guid? locacaoId)
        {
            ImovelId = imovelId;
            SolicitanteId = solicitanteId;
            Descricao = descricao;
            LocacaoId = locacaoId;
            Status = StatusSolicitacaoManutencao.Aberta;
        }

        private SolicitacaoManutencao() { Descricao = null!; }

        public static Result<SolicitacaoManutencao> Abrir(Guid imovelId, Guid solicitanteId, string? descricao, Guid? locacaoId = null)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                return Result<SolicitacaoManutencao>.Failure("Descrição é obrigatória.");

            return Result<SolicitacaoManutencao>.Success(new SolicitacaoManutencao(imovelId, solicitanteId, descricao.Trim(), locacaoId));
        }

        public Result<Orcamento> AdicionarOrcamento(Guid prestadorId, Dinheiro valor, string? descricao = null)
        {
            if (Status is StatusSolicitacaoManutencao.Concluida or StatusSolicitacaoManutencao.Cancelada)
                return Result<Orcamento>.Failure($"Não é possível adicionar orçamento a uma solicitação {Status}.");

            var orcamentoResult = Orcamento.Criar(prestadorId, valor, descricao);
            if (!orcamentoResult.IsSuccess)
                return Result<Orcamento>.Failure(orcamentoResult.Error!);

            _orcamentos.Add(orcamentoResult.Value!);

            if (Status == StatusSolicitacaoManutencao.Aberta)
                Status = StatusSolicitacaoManutencao.EmOrcamento;

            MarcarAtualizado();
            return Result<Orcamento>.Success(orcamentoResult.Value!);
        }

        public Result<bool> AprovarOrcamento(Guid orcamentoId)
        {
            var orcamento = _orcamentos.FirstOrDefault(o => o.Id == orcamentoId);

            if (orcamento is null)
                return Result<bool>.Failure("Orçamento não encontrado.");

            if (Status != StatusSolicitacaoManutencao.EmOrcamento)
                return Result<bool>.Failure("Só é possível aprovar orçamento enquanto a solicitação está em orçamento.");

            orcamento.Aprovar();

            foreach (var outro in _orcamentos.Where(o => o.Id != orcamentoId && o.Status == StatusOrcamento.Pendente))
                outro.Reprovar();

            OrcamentoAprovadoId = orcamentoId;
            Status = StatusSolicitacaoManutencao.Aprovada;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> IniciarExecucao()
        {
            if (Status != StatusSolicitacaoManutencao.Aprovada)
                return Result<bool>.Failure("Só é possível iniciar execução após aprovação de orçamento.");

            Status = StatusSolicitacaoManutencao.EmExecucao;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> ConcluirServico(DateTime dataConclusao)
        {
            if (Status != StatusSolicitacaoManutencao.EmExecucao)
                return Result<bool>.Failure("Só é possível concluir uma solicitação em execução.");

            Status = StatusSolicitacaoManutencao.Concluida;
            DataConclusao = dataConclusao;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> Cancelar(string motivo)
        {
            if (Status == StatusSolicitacaoManutencao.Concluida)
                return Result<bool>.Failure("Não é possível cancelar uma solicitação já concluída.");

            if (string.IsNullOrWhiteSpace(motivo))
                return Result<bool>.Failure("Motivo do cancelamento é obrigatório.");

            Status = StatusSolicitacaoManutencao.Cancelada;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }
    }
}
