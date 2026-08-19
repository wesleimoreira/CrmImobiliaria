using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Repasses.Queries
{
    public sealed record ObterRepassePorIdQuery(Guid Id) : IQuery<RepasseDetalheDto?>;

    public sealed record DespesaRepasseDto(string Descricao, decimal Valor);

    public sealed record RepasseDetalheDto(
        Guid Id, Guid LocacaoId, string Imovel, string Proprietario, int Mes, int Ano,
        decimal ValorAluguelRecebido, decimal TaxaAdministracao, decimal ValorTaxaAdministracao,
        List<DespesaRepasseDto> Despesas, decimal ValorLiquido, StatusRepasse Status,
        DateOnly? DataRepasse, string? Comprovante);
}
