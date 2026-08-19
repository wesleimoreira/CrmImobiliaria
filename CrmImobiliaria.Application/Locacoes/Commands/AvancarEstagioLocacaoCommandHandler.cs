using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Locacoes.Commands
{
    public sealed class AvancarEstagioLocacaoCommandHandler(ILocacaoRepository locacaoRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<AvancarEstagioLocacaoCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(AvancarEstagioLocacaoCommand command, CancellationToken cancellationToken = default)
        {
            var locacao = await locacaoRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (locacao is null)
                return Result<bool>.Failure("Locação não encontrada.");

            var resultado = command.Acao switch
            {
                AcaoLocacao.RegistrarVisita => locacao.RegistrarVisita(),
                AcaoLocacao.IniciarAnaliseCadastral => locacao.IniciarAnaliseCadastral(),
                AcaoLocacao.Aprovar => locacao.Aprovar(),
                AcaoLocacao.Reprovar => locacao.Reprovar(command.Texto ?? string.Empty),
                AcaoLocacao.RegistrarVistoriaEntrada => locacao.RegistrarVistoriaEntrada(),
                AcaoLocacao.EntregarChaves => locacao.EntregarChaves(),
                AcaoLocacao.Encerrar => locacao.Encerrar(command.Texto),
                AcaoLocacao.Cancelar => locacao.Cancelar(command.Texto ?? string.Empty),
                _ => Result<bool>.Failure("Ação inválida.")
            };

            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}
