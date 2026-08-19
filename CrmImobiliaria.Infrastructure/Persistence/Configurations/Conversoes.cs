using CrmImobiliaria.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CrmImobiliaria.Infrastructure.Persistence.Configurations
{
    // ValueConverters reutilizáveis para os Value Objects que viram uma única coluna.
    // Sempre reaproveita os Criar()/CriarDeTexto() do próprio VO no round-trip — nunca duplica validação aqui.
    internal static class Conversoes
    {
        public static readonly ValueConverter<Dinheiro, decimal> DinheiroConverter =
            new(v => v.Valor, v => Dinheiro.Criar(v).Value!);

        public static readonly ValueConverter<Area, decimal> AreaConverter =
            new(v => v.MetrosQuadrados, v => Area.Criar(v).Value!);

        public static readonly ValueConverter<Percentual, decimal> PercentualConverter =
            new(v => v.Valor, v => Percentual.Criar(v).Value!);

        public static readonly ValueConverter<Email, string> EmailConverter =
            new(v => v.Endereco, v => Email.Criar(v).Value!);

        public static readonly ValueConverter<Telefone, string> TelefoneConverter =
            new(v => v.Ddd + v.Numero, v => Telefone.Criar(v).Value!);

        public static readonly ValueConverter<CpfCnpj, string> CpfCnpjConverter =
            new(v => v.Numero, v => CpfCnpj.Criar(v).Value!);

        public static readonly ValueConverter<Creci, string> CreciConverter =
            new(v => v.ToString(), v => Creci.CriarDeTexto(v).Value!);

        public static readonly ValueConverter<CodigoImovel, string> CodigoImovelConverter =
            new(v => v.ToString(), v => CodigoImovel.CriarDeTexto(v).Value!);

        public static readonly ValueConverter<Competencia, int> CompetenciaConverter =
            new(v => v.Ano * 100 + v.Mes, v => Competencia.Criar(v % 100, v / 100).Value!);

        public static readonly ValueConverter<Dinheiro?, decimal?> DinheiroNuloConverter =
            new(v => v == null ? null : v.Valor, v => v == null ? null : Dinheiro.Criar(v.Value).Value!);

        public static readonly ValueConverter<Percentual?, decimal?> PercentualNuloConverter =
            new(v => v == null ? null : v.Valor, v => v == null ? null : Percentual.Criar(v.Value).Value!);

        public static readonly ValueConverter<Email?, string?> EmailNuloConverter =
            new(v => v == null ? null : v.Endereco, v => v == null ? null : Email.Criar(v).Value!);

        public static readonly ValueConverter<CpfCnpj?, string?> CpfCnpjNuloConverter =
            new(v => v == null ? null : v.Numero, v => v == null ? null : CpfCnpj.Criar(v).Value!);

        public static readonly ValueConverter<Telefone?, string?> TelefoneNuloConverter =
            new(v => v == null ? null : v.Ddd + v.Numero, v => v == null ? null : Telefone.Criar(v).Value!);
    }
}
