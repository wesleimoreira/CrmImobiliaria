using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Clientes.Commands
{
    public sealed class CriarClienteCommandHandler(
        IClienteRepository clienteRepository,
        ICorretorRepository corretorRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<CriarClienteCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(CriarClienteCommand command, CancellationToken cancellationToken = default)
        {
            var corretor = await corretorRepository.ObterPorIdAsync(command.CorretorResponsavelId, cancellationToken);
            if (corretor is null)
                return Result<Guid>.Failure("Corretor responsável não encontrado.");

            var telefoneResultado = Telefone.Criar(command.Telefone);
            if (!telefoneResultado.IsSuccess)
                return Result<Guid>.Failure(telefoneResultado.Error!);

            var emailResultado = Email.Criar(command.Email);
            if (!emailResultado.IsSuccess)
                return Result<Guid>.Failure(emailResultado.Error!);

            Telefone? whatsApp = null;
            if (!string.IsNullOrWhiteSpace(command.WhatsApp))
            {
                var whatsAppResultado = Telefone.Criar(command.WhatsApp);
                if (!whatsAppResultado.IsSuccess)
                    return Result<Guid>.Failure(whatsAppResultado.Error!);

                whatsApp = whatsAppResultado.Value;
            }

            CpfCnpj? documento = null;
            if (!string.IsNullOrWhiteSpace(command.Documento))
            {
                var documentoResultado = CpfCnpj.Criar(command.Documento);
                if (!documentoResultado.IsSuccess)
                    return Result<Guid>.Failure(documentoResultado.Error!);

                documento = documentoResultado.Value;
            }

            var clienteResultado = Cliente.Criar(
                command.Nome, telefoneResultado.Value!, emailResultado.Value!, command.Tipos,
                command.CorretorResponsavelId, command.Origem, documento, whatsApp,
                command.CampanhaEspecifica, command.Observacoes);

            if (!clienteResultado.IsSuccess)
                return Result<Guid>.Failure(clienteResultado.Error!);

            var cliente = clienteResultado.Value!;
            clienteRepository.Adicionar(cliente);
            await unitOfWork.SalvarAsync(cancellationToken);

            return Result<Guid>.Success(cliente.Id);
        }
    }
}
