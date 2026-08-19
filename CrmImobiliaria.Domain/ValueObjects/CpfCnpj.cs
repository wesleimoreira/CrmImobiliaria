using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.ValueObjects
{  
    public sealed class CpfCnpj : ValueObject
    {
        public string Numero { get; }
        public TipoDocumento Tipo { get; }

        private CpfCnpj(string numero, TipoDocumento tipo)
        {
            Numero = numero;
            Tipo = tipo;
        }

        public static Result<CpfCnpj> Criar(string? documento)
        {
            if (string.IsNullOrWhiteSpace(documento))
                return Result<CpfCnpj>.Failure("CPF/CNPJ é obrigatório.");

            var digitos = new string(documento.Where(char.IsDigit).ToArray());

            return digitos.Length switch
            {
                11 when EhCpfValido(digitos) => Result<CpfCnpj>.Success(new CpfCnpj(digitos, TipoDocumento.Cpf)),
                11 => Result<CpfCnpj>.Failure("CPF inválido."),
                14 when EhCnpjValido(digitos) => Result<CpfCnpj>.Success(new CpfCnpj(digitos, TipoDocumento.Cnpj)),
                14 => Result<CpfCnpj>.Failure("CNPJ inválido."),
                _ => Result<CpfCnpj>.Failure("Documento deve ter 11 (CPF) ou 14 (CNPJ) dígitos.")
            };
        }

        private static bool EhCpfValido(string cpf)
        {
            if (cpf.Distinct().Count() == 1) return false;

            var d1 = CalcularDigito(cpf[..9], [10, 9, 8, 7, 6, 5, 4, 3, 2]);
            var d2 = CalcularDigito(cpf[..9] + d1, [11, 10, 9, 8, 7, 6, 5, 4, 3, 2]);

            return cpf == cpf[..9] + d1 + d2;
        }

        private static bool EhCnpjValido(string cnpj)
        {
            if (cnpj.Distinct().Count() == 1) return false;

            var d1 = CalcularDigito(cnpj[..12], [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2]);
            var d2 = CalcularDigito(cnpj[..12] + d1, [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2]);

            return cnpj == cnpj[..12] + d1 + d2;
        }

        private static string CalcularDigito(string baseNumero, int[] pesos)
        {
            var soma = baseNumero.Select((c, i) => (c - '0') * pesos[i]).Sum();
            var resto = soma % 11;
            return (resto < 2 ? 0 : 11 - resto).ToString();
        }

        public string Formatado => Tipo == TipoDocumento.Cpf
            ? $"{Numero[..3]}.{Numero[3..6]}.{Numero[6..9]}-{Numero[9..]}"
            : $"{Numero[..2]}.{Numero[2..5]}.{Numero[5..8]}/{Numero[8..12]}-{Numero[12..]}";

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Numero;
        }

        public override string ToString() => Formatado;
    }
}
