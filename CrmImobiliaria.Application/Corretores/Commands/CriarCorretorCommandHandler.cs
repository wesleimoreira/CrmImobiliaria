using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Corretores.Commands
{
    public sealed class CriarCorretorCommandHandler(ICorretorRepository corretorRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<CriarCorretorCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(CriarCorretorCommand command, CancellationToken cancellationToken = default)
        {
            var creciResultado = Creci.Criar(command.CreciNumero, command.CreciTipo, command.CreciUf);
            if (!creciResultado.IsSuccess)
                return Result<Guid>.Failure(creciResultado.Error!);

            var telefoneResultado = Telefone.Criar(command.Telefone);
            if (!telefoneResultado.IsSuccess)
                return Result<Guid>.Failure(telefoneResultado.Error!);

            var emailResultado = Email.Criar(command.Email);
            if (!emailResultado.IsSuccess)
                return Result<Guid>.Failure(emailResultado.Error!);

            var corretorResultado = Corretor.Criar(
                command.Nome, creciResultado.Value!, telefoneResultado.Value!, emailResultado.Value!,
                equipe: command.Equipe);

            if (!corretorResultado.IsSuccess)
                return Result<Guid>.Failure(corretorResultado.Error!);

            var corretor = corretorResultado.Value!;
            corretorRepository.Adicionar(corretor);
            await unitOfWork.SalvarAsync(cancellationToken);

            return Result<Guid>.Success(corretor.Id);
        }
    }
}
