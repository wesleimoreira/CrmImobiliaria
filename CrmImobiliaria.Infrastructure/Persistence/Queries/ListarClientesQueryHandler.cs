using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Clientes.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ListarClientesQueryHandler(CrmDbContext context) : IQueryHandler<ListarClientesQuery, List<ClienteListaItemDto>>
    {
        public async Task<Result<List<ClienteListaItemDto>>> HandleAsync(ListarClientesQuery query, CancellationToken cancellationToken = default)
        {
            var consulta = context.Clientes.AsNoTracking()
                .Join(context.Corretores.AsNoTracking(), c => c.CorretorResponsavelId, cr => cr.Id, (c, cr) => new { Cliente = c, CorretorNome = cr.Nome });

            if (!string.IsNullOrWhiteSpace(query.Termo))
                consulta = consulta.Where(x => x.Cliente.Nome.Contains(query.Termo));

            // Seleciona os Value Objects inteiros (traduzível via HasConversion) e formata em memória
            // depois — acessar sub-propriedades deles (Telefone.Ddd, Email.Endereco...) direto na
            // query não é traduzível pelo EF, já que só a coluna inteira é convertida, não os campos.
            var brutos = await consulta
                .Select(x => new { x.Cliente.Id, x.Cliente.Nome, x.Cliente.Telefone, x.Cliente.Email, x.Cliente.Tipos, x.CorretorNome })
                .ToListAsync(cancellationToken);

            var itens = brutos
                .Select(x => new ClienteListaItemDto(x.Id, x.Nome, x.Telefone.Formatado, x.Email.Endereco, string.Join(", ", x.Tipos), x.CorretorNome))
                .ToList();

            return Result<List<ClienteListaItemDto>>.Success(itens);
        }
    }
}
