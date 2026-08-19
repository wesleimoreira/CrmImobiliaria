using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Empreendimentos.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ObterEmpreendimentoPorIdQueryHandler(CrmDbContext context) : IQueryHandler<ObterEmpreendimentoPorIdQuery, EmpreendimentoDetalheDto?>
    {
        public async Task<Result<EmpreendimentoDetalheDto?>> HandleAsync(ObterEmpreendimentoPorIdQuery query, CancellationToken cancellationToken = default)
        {
            var empreendimento = await context.Empreendimentos.AsNoTracking().FirstOrDefaultAsync(e => e.Id == query.Id, cancellationToken);
            if (empreendimento is null)
                return Result<EmpreendimentoDetalheDto?>.Success(null);

            var dto = new EmpreendimentoDetalheDto(
                empreendimento.Id, empreendimento.Nome, empreendimento.LoteadoraIncorporadora, empreendimento.Localizacao.ToString(),
                empreendimento.DataLancamento, empreendimento.TotalLotes, empreendimento.NumeroQuadras,
                empreendimento.PercentualComissao.Valor, empreendimento.CampanhaVigente, empreendimento.Status);

            return Result<EmpreendimentoDetalheDto?>.Success(dto);
        }
    }
}
