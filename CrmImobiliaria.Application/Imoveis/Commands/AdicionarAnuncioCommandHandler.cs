using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Imoveis.Commands
{
    public sealed class AdicionarAnuncioCommandHandler(IImovelRepository imovelRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<AdicionarAnuncioCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(AdicionarAnuncioCommand command, CancellationToken cancellationToken = default)
        {
            var imovel = await imovelRepository.ObterPorIdAsync(command.ImovelId, cancellationToken);
            if (imovel is null)
                return Result<Guid>.Failure("Imóvel não encontrado.");

            var valorResultado = Dinheiro.CriarPositivo(command.Valor);
            if (!valorResultado.IsSuccess)
                return Result<Guid>.Failure(valorResultado.Error!);

            RegraEstadia? regraEstadia = null;
            if (command.Tipo == TipoNegociacaoImovel.Temporada)
            {
                var regraResultado = RegraEstadia.Criar(command.EstadiaMinimaNoites ?? 1);
                if (!regraResultado.IsSuccess)
                    return Result<Guid>.Failure(regraResultado.Error!);

                regraEstadia = regraResultado.Value;
            }

            var ano = DateTime.UtcNow.Year;
            var proximoSequencial = await imovelRepository.ContarAnunciosPorTipoEAnoAsync(command.Tipo, ano, cancellationToken) + 1;
            var codigoResultado = CodigoImovel.Criar(command.Tipo, ano, proximoSequencial);
            if (!codigoResultado.IsSuccess)
                return Result<Guid>.Failure(codigoResultado.Error!);

            var anuncioResultado = imovel.AdicionarAnuncio(command.Tipo, codigoResultado.Value!, valorResultado.Value!, command.Exclusividade, regraEstadia);
            if (!anuncioResultado.IsSuccess)
                return Result<Guid>.Failure(anuncioResultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<Guid>.Success(anuncioResultado.Value!.Id);
        }
    }
}
