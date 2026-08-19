using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Corretores.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ObterCorretorPorIdQueryHandler(CrmDbContext context) : IQueryHandler<ObterCorretorPorIdQuery, CorretorDetalheDto?>
    {
        public async Task<Result<CorretorDetalheDto?>> HandleAsync(ObterCorretorPorIdQuery query, CancellationToken cancellationToken = default)
        {
            var corretor = await context.Corretores.AsNoTracking().FirstOrDefaultAsync(c => c.Id == query.Id, cancellationToken);
            if (corretor is null)
                return Result<CorretorDetalheDto?>.Success(null);

            var dto = new CorretorDetalheDto(
                corretor.Id, corretor.Nome, corretor.Creci.ToString()!, corretor.Telefone.Formatado, corretor.Email.Endereco,
                corretor.Equipe, corretor.GerenteId, corretor.Status, corretor.CriadoEm);

            return Result<CorretorDetalheDto?>.Success(dto);
        }
    }
}
