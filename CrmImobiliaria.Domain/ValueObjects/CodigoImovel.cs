using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.ValueObjects
{
    public sealed class CodigoImovel : ValueObject
    {
        public TipoNegociacaoImovel Tipo { get; }
        public int Ano { get; }
        public int Sequencial { get; }

        private CodigoImovel(TipoNegociacaoImovel tipo, int ano, int sequencial)
        {
            Tipo = tipo;
            Ano = ano;
            Sequencial = sequencial;
        }

        public static Result<CodigoImovel> Criar(TipoNegociacaoImovel tipo, int ano, int sequencial)
        {
            if (ano < 2000 || ano > 2100)
                return Result<CodigoImovel>.Failure("Ano inválido.");

            if (sequencial <= 0)
                return Result<CodigoImovel>.Failure("Sequencial deve ser maior que zero.");

            return Result<CodigoImovel>.Success(new CodigoImovel(tipo, ano, sequencial));
        }

        public static Result<CodigoImovel> CriarDeTexto(string? codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                return Result<CodigoImovel>.Failure("Código é obrigatório.");

            var partes = codigo.Trim().ToUpperInvariant().Split('-');

            if (partes.Length != 3)
                return Result<CodigoImovel>.Failure("Formato esperado: TIPO-ANO-SEQUENCIAL (ex: V-2026-00001).");

            TipoNegociacaoImovel? tipo = partes[0] switch
            {
                "V" => TipoNegociacaoImovel.Venda,
                "L" => TipoNegociacaoImovel.Locacao,
                "T" => TipoNegociacaoImovel.Temporada,
                _ => null
            };

            if (tipo is null)
                return Result<CodigoImovel>.Failure("Prefixo inválido. Use V, L ou T.");

            if (!int.TryParse(partes[1], out var ano) || !int.TryParse(partes[2], out var sequencial))
                return Result<CodigoImovel>.Failure("Ano ou sequencial inválido.");

            return Criar(tipo.Value, ano, sequencial);
        }

        private string Prefixo => Tipo switch
        {
            TipoNegociacaoImovel.Venda => "V",
            TipoNegociacaoImovel.Locacao => "L",
            TipoNegociacaoImovel.Temporada => "T",
            _ => throw new InvalidOperationException()
        };

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Tipo;
            yield return Ano;
            yield return Sequencial;
        }

        public override string ToString() => $"{Prefixo}-{Ano}-{Sequencial:D5}";
    }
}
