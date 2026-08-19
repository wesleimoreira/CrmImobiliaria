using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Clientes.Commands
{
    public sealed class AtualizarClienteCommandHandler(
        IClienteRepository clienteRepository,
        ICorretorRepository corretorRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<AtualizarClienteCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(AtualizarClienteCommand command, CancellationToken cancellationToken = default)
        {
            var cliente = await clienteRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (cliente is null)
                return Result<bool>.Failure("Cliente não encontrado.");

            var corretor = await corretorRepository.ObterPorIdAsync(command.CorretorResponsavelId, cancellationToken);
            if (corretor is null)
                return Result<bool>.Failure("Corretor responsável não encontrado.");

            var telefoneResultado = Telefone.Criar(command.Telefone);
            if (!telefoneResultado.IsSuccess)
                return Result<bool>.Failure(telefoneResultado.Error!);

            var emailResultado = Email.Criar(command.Email);
            if (!emailResultado.IsSuccess)
                return Result<bool>.Failure(emailResultado.Error!);

            Telefone? whatsApp = null;
            if (!string.IsNullOrWhiteSpace(command.WhatsApp))
            {
                var whatsAppResultado = Telefone.Criar(command.WhatsApp);
                if (!whatsAppResultado.IsSuccess)
                    return Result<bool>.Failure(whatsAppResultado.Error!);

                whatsApp = whatsAppResultado.Value;
            }

            if (!string.IsNullOrWhiteSpace(command.Documento))
            {
                var documentoResultado = CpfCnpj.Criar(command.Documento);
                if (!documentoResultado.IsSuccess)
                    return Result<bool>.Failure(documentoResultado.Error!);

                cliente.DefinirDocumento(documentoResultado.Value!);
            }

            cliente.AtualizarContato(telefoneResultado.Value!, emailResultado.Value!, whatsApp);
            cliente.DefinirCorretorResponsavel(command.CorretorResponsavelId);
            cliente.AtualizarObservacoes(command.Observacoes);

            foreach (var tipo in command.Tipos.Except(cliente.Tipos).ToList())
            {
                var resultado = cliente.AdicionarTipo(tipo);
                if (!resultado.IsSuccess)
                    return Result<bool>.Failure(resultado.Error!);
            }

            foreach (var tipo in cliente.Tipos.Except(command.Tipos).ToList())
            {
                var resultado = cliente.RemoverTipo(tipo);
                if (!resultado.IsSuccess)
                    return Result<bool>.Failure(resultado.Error!);
            }

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}
