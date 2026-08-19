using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.ValueObjects
{
    public sealed class Telefone : ValueObject
    {
        public string Ddd { get; }
        public string Numero { get; }

        private Telefone(string ddd, string numero)
        {
            Ddd = ddd;
            Numero = numero;
        }

        public static Result<Telefone> Criar(string? numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
                return Result<Telefone>.Failure("Telefone é obrigatório.");

            var digitos = new string(numero.Where(char.IsDigit).ToArray());

            if (digitos.Length is 12 or 13)      // veio com +55
                digitos = digitos[2..];

            if (digitos.Length is not (10 or 11))
                return Result<Telefone>.Failure("Telefone deve ter DDD + 8 ou 9 dígitos.");

            var ddd = digitos[..2];
            var restante = digitos[2..];

            if (!int.TryParse(ddd, out var dddNum) || dddNum is < 11 or > 99)
                return Result<Telefone>.Failure("DDD inválido.");

            return Result<Telefone>.Success(new Telefone(ddd, restante));
        }

        public string Formatado => Numero.Length == 9 ? $"({Ddd}) {Numero[..5]}-{Numero[5..]}" : $"({Ddd}) {Numero[..4]}-{Numero[4..]}";

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Ddd;
            yield return Numero;
        }

        public override string ToString() => Formatado;
    }
}
