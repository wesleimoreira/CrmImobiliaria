using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed class Lote : AggregateRoot
    {
        public Guid EmpreendimentoId { get; private set; }
        public string Quadra { get; private set; }
        public string Numero { get; private set; }
        public Area Area { get; private set; }
        public Dinheiro Valor { get; private set; }
        public Dinheiro EntradaMinima { get; private set; }
        public int ParcelamentoMaximo { get; private set; }
        public Dinheiro? ValorPromocional { get; private set; }
        public StatusLote Status { get; private set; }

        public Dinheiro ValorVigente => ValorPromocional ?? Valor;

        private Lote(Guid empreendimentoId, string quadra, string numero, Area area, Dinheiro valor, Dinheiro entradaMinima, int parcelamentoMaximo, Dinheiro? valorPromocional)
        {
            EmpreendimentoId = empreendimentoId;
            Quadra = quadra;
            Numero = numero;
            Area = area;
            Valor = valor;
            EntradaMinima = entradaMinima;
            ParcelamentoMaximo = parcelamentoMaximo;
            ValorPromocional = valorPromocional;
            Status = StatusLote.Disponivel;
        }

        private Lote()
        {
            Quadra = null!;
            Numero = null!;
            Area = null!;
            Valor = null!;
            EntradaMinima = null!;
        }

        public static Result<Lote> Criar(Guid empreendimentoId, string? quadra, string? numero, Area area, Dinheiro valor, Dinheiro entradaMinima, int parcelamentoMaximo, Dinheiro? valorPromocional = null)
        {
            if (string.IsNullOrWhiteSpace(quadra))
                return Result<Lote>.Failure("Quadra é obrigatória.");

            if (string.IsNullOrWhiteSpace(numero))
                return Result<Lote>.Failure("Número do lote é obrigatório.");

            if (parcelamentoMaximo <= 0)
                return Result<Lote>.Failure("Parcelamento máximo deve ser maior que zero.");

            if (entradaMinima.Valor > valor.Valor)
                return Result<Lote>.Failure("Entrada mínima não pode ser maior que o valor do lote.");

            if (valorPromocional is not null && valorPromocional.Valor >= valor.Valor)
                return Result<Lote>.Failure("Valor promocional deve ser menor que o valor cheio.");

            return Result<Lote>.Success(new Lote(empreendimentoId, quadra.Trim(), numero.Trim(), area, valor, entradaMinima, parcelamentoMaximo, valorPromocional));
        }

        public Result<bool> Reservar()
        {
            if (Status != StatusLote.Disponivel)
                return Result<bool>.Failure($"Só é possível reservar um lote disponível. Status atual: {Status}.");

            Status = StatusLote.Reservado;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> IniciarProposta()
        {
            if (Status != StatusLote.Reservado)
                return Result<bool>.Failure("Só é possível propor um lote reservado.");

            Status = StatusLote.EmProposta;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> Vender()
        {
            if (Status != StatusLote.EmProposta)
                return Result<bool>.Failure("Só é possível vender um lote em proposta.");

            Status = StatusLote.Vendido;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> LiberarReserva()
        {
            if (Status is not (StatusLote.Reservado or StatusLote.EmProposta))
                return Result<bool>.Failure($"Não há reserva/proposta ativa para liberar. Status atual: {Status}.");

            Status = StatusLote.Disponivel;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> Distratar(string motivo)
        {
            if (Status != StatusLote.Vendido)
                return Result<bool>.Failure("Só é possível distratar um lote vendido.");

            if (string.IsNullOrWhiteSpace(motivo))
                return Result<bool>.Failure("Motivo do distrato é obrigatório.");

            Status = StatusLote.Distrato;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> Bloquear()
        {
            if (Status is StatusLote.Vendido)
                return Result<bool>.Failure("Não é possível bloquear um lote vendido.");

            Status = StatusLote.Bloqueado;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> Desbloquear()
        {
            if (Status != StatusLote.Bloqueado)
                return Result<bool>.Failure("Lote não está bloqueado.");

            Status = StatusLote.Disponivel;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }
    }
}
