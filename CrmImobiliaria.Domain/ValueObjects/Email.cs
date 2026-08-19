using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Shared;
using System.Text.RegularExpressions;

namespace CrmImobiliaria.Domain.ValueObjects
{
    public sealed partial class Email : ValueObject
    {
        public string Endereco { get; }

        private Email(string endereco) => Endereco = endereco;

        public static Result<Email> Criar(string? endereco)
        {
            if (string.IsNullOrWhiteSpace(endereco))
                return Result<Email>.Failure("E-mail é obrigatório.");

            endereco = endereco.Trim().ToLowerInvariant();

            if (!EmailRegex().IsMatch(endereco))
                return Result<Email>.Failure("E-mail inválido.");

            return Result<Email>.Success(new Email(endereco));
        }

        [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase)]
        private static partial Regex EmailRegex();

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Endereco;
        }

        public override string ToString() => Endereco;
    }
}
