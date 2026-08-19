using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed class ReservaLote : AggregateRoot
    {
        public Guid LoteId { get; private set; }
        public Guid ClienteId { get; private set; }
        public Guid CorretorId { get; private set; }
        public DateOnly DataReserva { get; private set; }
        public DateOnly DataExpiracao { get; private set; }
        public StatusReservaLote Status { get; private set; }

        private ReservaLote(Guid loteId, Guid clienteId, Guid corretorId, DateOnly dataReserva, DateOnly dataExpiracao)
        {
            LoteId = loteId;
            ClienteId = clienteId;
            CorretorId = corretorId;
            DataReserva = dataReserva;
            DataExpiracao = dataExpiracao;
            Status = StatusReservaLote.Ativa;
        }

        private ReservaLote() { }

        public static Result<ReservaLote> Criar(Guid loteId, Guid clienteId, Guid corretorId, DateOnly dataReserva, int prazoDias)
        {
            if (prazoDias <= 0)
                return Result<ReservaLote>.Failure("Prazo da reserva deve ser maior que zero dias.");

            return Result<ReservaLote>.Success(new ReservaLote(loteId, clienteId, corretorId, dataReserva, dataReserva.AddDays(prazoDias)));
        }

        public bool EstaExpirada(DateOnly referencia) => Status == StatusReservaLote.Ativa && referencia > DataExpiracao;

        public Result<bool> Expirar()
        {
            if (Status != StatusReservaLote.Ativa)
                return Result<bool>.Failure("Só é possível expirar uma reserva ativa.");

            Status = StatusReservaLote.Expirada;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> Converter()
        {
            if (Status != StatusReservaLote.Ativa)
                return Result<bool>.Failure("Só é possível converter uma reserva ativa em proposta.");

            Status = StatusReservaLote.Convertida;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> Cancelar()
        {
            if (Status == StatusReservaLote.Convertida)
                return Result<bool>.Failure("Não é possível cancelar uma reserva já convertida.");

            Status = StatusReservaLote.Cancelada;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }
    }
}
