using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Visitas.Commands
{
    public sealed class AgendarVisitaCommandHandler(
        IVisitaRepository visitaRepository,
        IClienteRepository clienteRepository,
        IImovelRepository imovelRepository,
        ICorretorRepository corretorRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<AgendarVisitaCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(AgendarVisitaCommand command, CancellationToken cancellationToken = default)
        {
            var cliente = await clienteRepository.ObterPorIdAsync(command.ClienteId, cancellationToken);
            if (cliente is null)
                return Result<Guid>.Failure("Cliente não encontrado.");

            var imovel = await imovelRepository.ObterPorIdAsync(command.ImovelId, cancellationToken);
            if (imovel is null)
                return Result<Guid>.Failure("Imóvel não encontrado.");

            if (imovel.Anuncios.All(a => a.Id != command.AnuncioImovelId))
                return Result<Guid>.Failure("Anúncio não encontrado para este imóvel.");

            var corretor = await corretorRepository.ObterPorIdAsync(command.CorretorId, cancellationToken);
            if (corretor is null)
                return Result<Guid>.Failure("Corretor não encontrado.");

            var visitaResultado = Visita.Agendar(command.ClienteId, command.ImovelId, command.AnuncioImovelId, command.CorretorId, command.DataHora);
            if (!visitaResultado.IsSuccess)
                return Result<Guid>.Failure(visitaResultado.Error!);

            var visita = visitaResultado.Value!;
            visitaRepository.Adicionar(visita);
            await unitOfWork.SalvarAsync(cancellationToken);

            return Result<Guid>.Success(visita.Id);
        }
    }
}
