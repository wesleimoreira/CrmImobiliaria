using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Leads.Commands
{
    public sealed class AvancarEstagioLeadCommandHandler(ILeadRepository leadRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<AvancarEstagioLeadCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(AvancarEstagioLeadCommand command, CancellationToken cancellationToken = default)
        {
            var lead = await leadRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (lead is null)
                return Result<bool>.Failure("Lead não encontrado.");

            var resultado = command.EstagioAlvo switch
            {
                EstagioFunil.Contato => lead.RegistrarContato(command.Observacao),
                EstagioFunil.Qualificado => lead.Qualificar(command.Observacao),
                EstagioFunil.Visita => lead.RegistrarVisita(command.Observacao),
                EstagioFunil.Proposta => lead.RegistrarProposta(command.Observacao),
                EstagioFunil.Negociacao => lead.IniciarNegociacao(command.Observacao),
                EstagioFunil.Fechado => lead.Fechar(command.Observacao),
                _ => Result<bool>.Failure("Estágio inválido.")
            };

            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}
