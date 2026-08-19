using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Vistorias.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ObterVistoriaPorIdQueryHandler(CrmDbContext context) : IQueryHandler<ObterVistoriaPorIdQuery, VistoriaDetalheDto?>
    {
        public async Task<Result<VistoriaDetalheDto?>> HandleAsync(ObterVistoriaPorIdQuery query, CancellationToken cancellationToken = default)
        {
            var vistoria = await context.Vistorias.AsNoTracking().FirstOrDefaultAsync(v => v.Id == query.Id, cancellationToken);
            if (vistoria is null)
                return Result<VistoriaDetalheDto?>.Success(null);

            var imovel = await context.Imoveis.AsNoTracking().FirstOrDefaultAsync(i => i.Id == vistoria.ImovelId, cancellationToken);
            var responsavel = await context.Corretores.AsNoTracking().FirstOrDefaultAsync(c => c.Id == vistoria.ResponsavelId, cancellationToken);

            var dto = new VistoriaDetalheDto(
                vistoria.Id, imovel?.Endereco.ToString() ?? "—", vistoria.Tipo, vistoria.DataAgendada, vistoria.DataRealizada,
                responsavel?.Nome ?? "—", vistoria.Status, vistoria.Observacoes, vistoria.Fotos.ToList());

            return Result<VistoriaDetalheDto?>.Success(dto);
        }
    }
}
