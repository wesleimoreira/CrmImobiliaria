using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed class ReservaTemporada : AggregateRoot
    {
        public Guid ImovelId { get; private set; }
        public Guid AnuncioImovelId { get; private set; }
        public Guid ClienteId { get; private set; }
        public PeriodoContrato Periodo { get; private set; }   // DataInicial = check-in, DataFinal = check-out
        public Dinheiro ValorDiaria { get; private set; }       // snapshot no momento da reserva
        public Dinheiro ValorTotal { get; private set; }
        public StatusReserva Status { get; private set; }
        public string? Observacoes { get; private set; }

        private ReservaTemporada(Guid imovelId, Guid anuncioImovelId, Guid clienteId, PeriodoContrato periodo, Dinheiro valorDiaria, Dinheiro valorTotal, string? observacoes)
        {
            ImovelId = imovelId;
            AnuncioImovelId = anuncioImovelId;
            ClienteId = clienteId;
            Periodo = periodo;
            ValorDiaria = valorDiaria;
            ValorTotal = valorTotal;
            Observacoes = observacoes;
            Status = StatusReserva.Pendente;
        }

        private ReservaTemporada()
        {
            Periodo = null!;
            ValorDiaria = null!;
            ValorTotal = null!;
        }

        // regraEstadia e valorDiaria vêm do AnuncioImovel correspondente (buscados pela Application) —
        // usados aqui só pra validar/calcular, não ficam guardados por referência.
        public static Result<ReservaTemporada> Criar(Guid imovelId, Guid anuncioImovelId, Guid clienteId, DateOnly checkIn, DateOnly checkOut, RegraEstadia regraEstadia, Dinheiro valorDiaria, string? observacoes = null)
        {
            var periodoResult = PeriodoContrato.Criar(checkIn, checkOut);

            if (!periodoResult.IsSuccess)
                return Result<ReservaTemporada>.Failure(periodoResult.Error!);

            var periodo = periodoResult.Value!;

            var validacaoEstadia = regraEstadia.ValidarEstadia(checkIn, checkOut);

            if (!validacaoEstadia.IsSuccess)
                return Result<ReservaTemporada>.Failure(validacaoEstadia.Error!);

            var valorTotal = Dinheiro.Criar(valorDiaria.Valor * periodo.DuracaoEmDias).Value!;

            return Result<ReservaTemporada>.Success(new ReservaTemporada(
                imovelId, anuncioImovelId, clienteId, periodo, valorDiaria, valorTotal, observacoes));
        }

        public Result<bool> Confirmar()
        {
            if (Status != StatusReserva.Pendente)
                return Result<bool>.Failure("Só é possível confirmar uma reserva pendente.");

            Status = StatusReserva.Confirmada;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> Cancelar(string? motivo = null)
        {
            if (Status is StatusReserva.Concluida or StatusReserva.Cancelada)
                return Result<bool>.Failure($"Não é possível cancelar uma reserva {Status}.");

            Status = StatusReserva.Cancelada;

            if (motivo is not null)
                Observacoes = string.IsNullOrWhiteSpace(Observacoes) ? $"Cancelada: {motivo}" : $"{Observacoes} | Cancelada: {motivo}";

            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> Concluir()
        {
            if (Status != StatusReserva.Confirmada)
                return Result<bool>.Failure("Só é possível concluir uma reserva confirmada.");

            Status = StatusReserva.Concluida;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        // Usado pela Application para checar sobreposição de datas contra as demais reservas do mesmo anúncio
        public bool Sobrepoe(DateOnly checkIn, DateOnly checkOut) => Status is StatusReserva.Pendente or StatusReserva.Confirmada && checkIn < Periodo.DataFinal && checkOut > Periodo.DataInicial;
    }
}
