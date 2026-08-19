using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.ValueObjects
{
    public sealed class RegraEstadia : ValueObject
    {
        public int EstadiaMinimaNoites { get; }
        public IReadOnlySet<DayOfWeek> DiasCheckinPermitidos { get; }
        public IReadOnlySet<DayOfWeek> DiasCheckoutPermitidos { get; }

        private RegraEstadia(int estadiaMinimaNoites, IReadOnlySet<DayOfWeek> diasCheckin, IReadOnlySet<DayOfWeek> diasCheckout)
        {
            EstadiaMinimaNoites = estadiaMinimaNoites;
            DiasCheckinPermitidos = diasCheckin;
            DiasCheckoutPermitidos = diasCheckout;
        }

        public static Result<RegraEstadia> Criar(int estadiaMinimaNoites, IEnumerable<DayOfWeek>? diasCheckinPermitidos = null, IEnumerable<DayOfWeek>? diasCheckoutPermitidos = null)
        {
            if (estadiaMinimaNoites <= 0)
                return Result<RegraEstadia>.Failure("Estadia mínima deve ser de pelo menos 1 noite.");

            // vazio/null = sem restrição, qualquer dia serve
            var checkin = (diasCheckinPermitidos ?? []).ToHashSet();
            var checkout = (diasCheckoutPermitidos ?? []).ToHashSet();

            return Result<RegraEstadia>.Success(new RegraEstadia(estadiaMinimaNoites, checkin, checkout));
        }

        public bool CheckinPermitido(DayOfWeek dia) => DiasCheckinPermitidos.Count == 0 || DiasCheckinPermitidos.Contains(dia);

        public bool CheckoutPermitido(DayOfWeek dia) => DiasCheckoutPermitidos.Count == 0 || DiasCheckoutPermitidos.Contains(dia);

        public Result<bool> ValidarEstadia(DateOnly checkin, DateOnly checkout)
        {
            if (checkout <= checkin)
                return Result<bool>.Failure("Data de checkout deve ser após o checkin.");

            var noites = checkout.DayNumber - checkin.DayNumber;

            if (noites < EstadiaMinimaNoites)
                return Result<bool>.Failure($"Estadia mínima é de {EstadiaMinimaNoites} noite(s).");

            if (!CheckinPermitido(checkin.DayOfWeek))
                return Result<bool>.Failure($"Checkin não é permitido às {checkin.DayOfWeek}.");

            if (!CheckoutPermitido(checkout.DayOfWeek))
                return Result<bool>.Failure($"Checkout não é permitido às {checkout.DayOfWeek}.");

            return Result<bool>.Success(true);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return EstadiaMinimaNoites;
            foreach (var d in DiasCheckinPermitidos.OrderBy(x => x)) yield return d;
            foreach (var d in DiasCheckoutPermitidos.OrderBy(x => x)) yield return d;
        }
    }
}
