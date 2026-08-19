using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Prestadores.Commands
{
    public sealed class CriarPrestadorCommandHandler(IPrestadorRepository prestadorRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<CriarPrestadorCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(CriarPrestadorCommand command, CancellationToken cancellationToken = default)
        {
            var telefoneResultado = Telefone.Criar(command.Telefone);
            if (!telefoneResultado.IsSuccess)
                return Result<Guid>.Failure(telefoneResultado.Error!);

            var emailResultado = Email.Criar(command.Email);
            if (!emailResultado.IsSuccess)
                return Result<Guid>.Failure(emailResultado.Error!);

            var prestadorResultado = Prestador.Criar(command.Nome, telefoneResultado.Value!, emailResultado.Value, command.Especialidade);
            if (!prestadorResultado.IsSuccess)
                return Result<Guid>.Failure(prestadorResultado.Error!);

            var prestador = prestadorResultado.Value!;
            prestadorRepository.Adicionar(prestador);
            await unitOfWork.SalvarAsync(cancellationToken);

            return Result<Guid>.Success(prestador.Id);
        }
    }
}
