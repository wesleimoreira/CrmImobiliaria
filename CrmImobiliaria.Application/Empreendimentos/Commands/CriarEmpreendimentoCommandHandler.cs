using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Empreendimentos.Commands
{
    public sealed class CriarEmpreendimentoCommandHandler(IEmpreendimentoRepository empreendimentoRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<CriarEmpreendimentoCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(CriarEmpreendimentoCommand command, CancellationToken cancellationToken = default)
        {
            var enderecoResultado = Endereco.Criar(command.Logradouro, command.Numero, command.Complemento, command.Bairro, command.Cidade, command.Uf, command.Cep);
            if (!enderecoResultado.IsSuccess)
                return Result<Guid>.Failure(enderecoResultado.Error!);

            var percentualResultado = Percentual.Criar(command.PercentualComissao);
            if (!percentualResultado.IsSuccess)
                return Result<Guid>.Failure(percentualResultado.Error!);

            var empreendimentoResultado = Empreendimento.Criar(
                command.Nome, command.LoteadoraIncorporadora, enderecoResultado.Value!, command.TotalLotes, command.NumeroQuadras,
                percentualResultado.Value!, command.DataLancamento);

            if (!empreendimentoResultado.IsSuccess)
                return Result<Guid>.Failure(empreendimentoResultado.Error!);

            var empreendimento = empreendimentoResultado.Value!;
            empreendimentoRepository.Adicionar(empreendimento);
            await unitOfWork.SalvarAsync(cancellationToken);

            return Result<Guid>.Success(empreendimento.Id);
        }
    }
}
