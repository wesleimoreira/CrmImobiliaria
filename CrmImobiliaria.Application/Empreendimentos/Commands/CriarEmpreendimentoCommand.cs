using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Empreendimentos.Commands
{
    public sealed record CriarEmpreendimentoCommand(
        string? Nome,
        string? LoteadoraIncorporadora,
        string Logradouro,
        string Numero,
        string? Complemento,
        string Bairro,
        string Cidade,
        string Uf,
        string Cep,
        int TotalLotes,
        int NumeroQuadras,
        decimal PercentualComissao,
        DateOnly? DataLancamento) : ICommand<Guid>;
}
