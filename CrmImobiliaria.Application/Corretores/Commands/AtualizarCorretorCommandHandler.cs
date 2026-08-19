using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Corretores.Commands
{
    public sealed class AtualizarCorretorCommandHandler(ICorretorRepository corretorRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<AtualizarCorretorCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(AtualizarCorretorCommand command, CancellationToken cancellationToken = default)
        {
            var corretor = await corretorRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (corretor is null)
                return Result<bool>.Failure("Corretor não encontrado.");

            var telefoneResultado = Telefone.Criar(command.Telefone);
            if (!telefoneResultado.IsSuccess)
                return Result<bool>.Failure(telefoneResultado.Error!);

            var emailResultado = Email.Criar(command.Email);
            if (!emailResultado.IsSuccess)
                return Result<bool>.Failure(emailResultado.Error!);

            corretor.AtualizarContato(telefoneResultado.Value!, emailResultado.Value!);
            corretor.DefinirEquipe(command.Equipe);

            if (command.GerenteId is { } gerenteId)
            {
                var resultado = corretor.DefinirGerente(gerenteId);
                if (!resultado.IsSuccess)
                    return Result<bool>.Failure(resultado.Error!);
            }
            else
            {
                corretor.RemoverGerente();
            }

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}
