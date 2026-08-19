using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Clientes.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ObterClientePorIdQueryHandler(CrmDbContext context) : IQueryHandler<ObterClientePorIdQuery, ClienteDetalheDto?>
    {
        public async Task<Result<ClienteDetalheDto?>> HandleAsync(ObterClientePorIdQuery query, CancellationToken cancellationToken = default)
        {
            var cliente = await context.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == query.Id, cancellationToken);
            if (cliente is null)
                return Result<ClienteDetalheDto?>.Success(null);

            var perfis = cliente.PerfisInteresse
                .Select(p => new PerfilInteresseDto(
                    p.Id, p.TipoNegociacao, p.TipoImovel, p.LocalizacaoDesejada,
                    p.FaixaPreco.Minimo.Valor, p.FaixaPreco.Maximo.Valor, p.NumeroQuartos, p.FormaPagamento, p.Observacoes))
                .ToList();

            var dto = new ClienteDetalheDto(
                cliente.Id, cliente.Nome, cliente.Documento?.Formatado, cliente.Telefone.Formatado,
                cliente.WhatsApp?.Formatado, cliente.Email.Endereco, cliente.Tipos.ToList(),
                cliente.CorretorResponsavelId, cliente.Origem, cliente.CampanhaEspecifica, cliente.Observacoes,
                cliente.CriadoEm, cliente.UltimoContato, cliente.ProximoContato, perfis);

            return Result<ClienteDetalheDto?>.Success(dto);
        }
    }
}
