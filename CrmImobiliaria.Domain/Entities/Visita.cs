using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed class Visita : AggregateRoot
    {
        public Guid ClienteId { get; private set; }
        public Guid ImovelId { get; private set; }
        public Guid AnuncioImovelId { get; private set; }
        public Guid CorretorId { get; private set; }
        public Guid? LeadId { get; private set; }
        public DateTime DataHora { get; private set; }
        public StatusVisita Status { get; private set; }
        public string? Feedback { get; private set; }

        private Visita(Guid clienteId, Guid imovelId, Guid anuncioImovelId, Guid corretorId, DateTime dataHora, Guid? leadId)
        {
            ClienteId = clienteId;
            ImovelId = imovelId;
            AnuncioImovelId = anuncioImovelId;
            CorretorId = corretorId;
            LeadId = leadId;
            DataHora = dataHora;
            Status = StatusVisita.Agendada;
        }

        private Visita() { }

        public static Result<Visita> Agendar(Guid clienteId, Guid imovelId, Guid anuncioImovelId, Guid corretorId, DateTime dataHora, Guid? leadId = null) => Result<Visita>.Success(new Visita(clienteId, imovelId, anuncioImovelId, corretorId, dataHora, leadId));

        public Result<bool> Confirmar()
        {
            if (Status != StatusVisita.Agendada)
                return Result<bool>.Failure("Só é possível confirmar uma visita agendada.");

            Status = StatusVisita.Confirmada;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> Reagendar(DateTime novaDataHora)
        {
            if (Status is StatusVisita.Realizada or StatusVisita.Cancelada)
                return Result<bool>.Failure($"Não é possível reagendar uma visita {Status}.");

            DataHora = novaDataHora;
            Status = StatusVisita.Agendada;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> RegistrarRealizada(string? feedback = null)
        {
            if (Status is not (StatusVisita.Agendada or StatusVisita.Confirmada))
                return Result<bool>.Failure($"Não é possível marcar como realizada uma visita {Status}.");

            Status = StatusVisita.Realizada;
            Feedback = feedback;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> MarcarNaoCompareceu(string? observacao = null)
        {
            if (Status is not (StatusVisita.Agendada or StatusVisita.Confirmada))
                return Result<bool>.Failure($"Não é possível marcar como não compareceu uma visita {Status}.");

            Status = StatusVisita.NaoCompareceu;
            Feedback = observacao;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> Cancelar(string? motivo = null)
        {
            if (Status is StatusVisita.Realizada or StatusVisita.Cancelada)
                return Result<bool>.Failure($"Não é possível cancelar uma visita {Status}.");

            Status = StatusVisita.Cancelada;
            Feedback = motivo;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }
    }
}
