using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.ValueObjects
{
    public sealed class Creci : ValueObject
    {
        public string Numero { get; }
        public TipoCreci Tipo { get; }
        public string Uf { get; }

        private Creci(string numero, TipoCreci tipo, string uf)
        {
            Numero = numero;
            Tipo = tipo;
            Uf = uf;
        }

        public static Result<Creci> Criar(string? numero, TipoCreci tipo, string? uf)
        {
            if (string.IsNullOrWhiteSpace(numero) || !numero.All(char.IsDigit))
                return Result<Creci>.Failure("Número do CRECI deve conter apenas dígitos.");

            if (string.IsNullOrWhiteSpace(uf) || uf.Trim().Length != 2)
                return Result<Creci>.Failure("UF inválida.");

            return Result<Creci>.Success(new Creci(numero.Trim(), tipo, uf.Trim().ToUpperInvariant()));
        }

        // aceita formato "12345-F/SP" ou "12345-J/SP"
        public static Result<Creci> CriarDeTexto(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return Result<Creci>.Failure("CRECI é obrigatório.");

            var partesUf = texto.Trim().ToUpperInvariant().Split('/');

            if (partesUf.Length != 2)
                return Result<Creci>.Failure("Formato esperado: NUMERO-F/UF (ex: 12345-F/SP).");

            var partesNumero = partesUf[0].Split('-');

            if (partesNumero.Length != 2)
                return Result<Creci>.Failure("Formato esperado: NUMERO-F/UF (ex: 12345-F/SP).");

            TipoCreci? tipo = partesNumero[1] switch
            {
                "F" => TipoCreci.PessoaFisica,
                "J" => TipoCreci.PessoaJuridica,
                _ => null
            };

            if (tipo is null)
                return Result<Creci>.Failure("Tipo deve ser F (Pessoa Física) ou J (Pessoa Jurídica).");

            return Criar(partesNumero[0], tipo.Value, partesUf[1]);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Numero;
            yield return Tipo;
            yield return Uf;
        }

        public override string ToString() => $"{Numero}-{(Tipo == TipoCreci.PessoaFisica ? "F" : "J")}/{Uf}";
    }
}
