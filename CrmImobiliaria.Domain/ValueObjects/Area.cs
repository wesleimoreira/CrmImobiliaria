using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.ValueObjects
{
    public sealed class Area : ValueObject
    {
        public decimal MetrosQuadrados { get; }

        private Area(decimal metrosQuadrados) => MetrosQuadrados = metrosQuadrados;

        public static Result<Area> Criar(decimal metrosQuadrados)
        {
            if (metrosQuadrados <= 0)
                return Result<Area>.Failure("Área deve ser maior que zero.");

            return Result<Area>.Success(new Area(Math.Round(metrosQuadrados, 2, MidpointRounding.AwayFromZero)));
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return MetrosQuadrados;
        }

        public override string ToString() => $"{MetrosQuadrados:0.##} m²";
    }
}
