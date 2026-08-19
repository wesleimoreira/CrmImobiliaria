using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.ValueObjects
{
    public sealed class Endereco : ValueObject
    {
        public string Logradouro { get; }
        public string Numero { get; }
        public string? Complemento { get; }
        public string Bairro { get; }
        public string Cidade { get; }
        public string Uf { get; }
        public string Cep { get; }

        private Endereco(string logradouro, string numero, string? complemento, string bairro, string cidade, string uf, string cep)
        {
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            Uf = uf;
            Cep = cep;
        }

        public static Result<Endereco> Criar(string logradouro, string numero, string? complemento, string bairro, string cidade, string uf, string cep)
        {
            if (string.IsNullOrWhiteSpace(logradouro))
                return Result<Endereco>.Failure("Logradouro é obrigatório.");

            if (string.IsNullOrWhiteSpace(bairro))
                return Result<Endereco>.Failure("Bairro é obrigatório.");

            if (string.IsNullOrWhiteSpace(cidade))
                return Result<Endereco>.Failure("Cidade é obrigatória.");

            if (uf is not { Length: 2 })
                return Result<Endereco>.Failure("UF deve ter 2 letras.");

            var cepDigitos = new string([.. cep.Where(char.IsDigit)]);

            if (cepDigitos.Length != 8)
                return Result<Endereco>.Failure("CEP inválido.");

            return Result<Endereco>.Success(new Endereco(logradouro.Trim(), numero.Trim(), complemento?.Trim(), bairro.Trim(), cidade.Trim(), uf.Trim().ToUpperInvariant(), cepDigitos));
        }

        public string CepFormatado => $"{Cep[..5]}-{Cep[5..]}";

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Logradouro;
            yield return Numero;
            yield return Complemento;
            yield return Bairro;
            yield return Cidade;
            yield return Uf;
            yield return Cep;
        }

        public override string ToString() => $"{Logradouro}, {Numero} - {Bairro}, {Cidade}/{Uf}";
    }
}
