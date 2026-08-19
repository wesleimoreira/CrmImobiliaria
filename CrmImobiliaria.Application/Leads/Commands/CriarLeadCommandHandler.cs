using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Leads.Commands
{
    public sealed class CriarLeadCommandHandler(
        ILeadRepository leadRepository,
        IClienteRepository clienteRepository,
        ICorretorRepository corretorRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<CriarLeadCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(CriarLeadCommand command, CancellationToken cancellationToken = default)
        {
            var cliente = await clienteRepository.ObterPorIdAsync(command.ClienteId, cancellationToken);
            if (cliente is null)
                return Result<Guid>.Failure("Cliente não encontrado.");

            var corretor = await corretorRepository.ObterPorIdAsync(command.CorretorId, cancellationToken);
            if (corretor is null)
                return Result<Guid>.Failure("Corretor não encontrado.");

            var leadResultado = Lead.Criar(command.ClienteId, command.CorretorId);
            if (!leadResultado.IsSuccess)
                return Result<Guid>.Failure(leadResultado.Error!);

            var lead = leadResultado.Value!;
            leadRepository.Adicionar(lead);
            await unitOfWork.SalvarAsync(cancellationToken);

            return Result<Guid>.Success(lead.Id);
        }
    }
}
