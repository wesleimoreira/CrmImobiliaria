using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Vendas.Commands
{
    public sealed class FecharVendaCommandHandler(
        IVendaRepository vendaRepository,
        IPropostaRepository propostaRepository,
        IImovelRepository imovelRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<FecharVendaCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(FecharVendaCommand command, CancellationToken cancellationToken = default)
        {
            var proposta = await propostaRepository.ObterPorIdAsync(command.PropostaId, cancellationToken);
            if (proposta is null)
                return Result<Guid>.Failure("Proposta não encontrada.");

            if (proposta.Status != StatusProposta.Aceita)
                return Result<Guid>.Failure("Só é possível fechar a venda de uma proposta aceita.");

            if (!proposta.EhParaImovel)
                return Result<Guid>.Failure("Fechamento de venda de lote é feito no módulo de Loteamentos.");

            if (await vendaRepository.ExisteParaPropostaAsync(proposta.Id, cancellationToken))
                return Result<Guid>.Failure("Já existe uma venda registrada para esta proposta.");

            var imovel = await imovelRepository.ObterPorIdAsync(proposta.ImovelId!.Value, cancellationToken);
            if (imovel is null)
                return Result<Guid>.Failure("Imóvel não encontrado.");

            DadosFinanciamento? financiamento = null;
            if (command.FormaPagamento == FormaPagamento.FinanciamentoBancario)
            {
                var valorFinanciadoResultado = Dinheiro.CriarPositivo(command.ValorFinanciado ?? 0);
                if (!valorFinanciadoResultado.IsSuccess)
                    return Result<Guid>.Failure(valorFinanciadoResultado.Error!);

                var financiamentoResultado = DadosFinanciamento.Criar(command.BancoFinanciamento, valorFinanciadoResultado.Value!);
                if (!financiamentoResultado.IsSuccess)
                    return Result<Guid>.Failure(financiamentoResultado.Error!);

                financiamento = financiamentoResultado.Value;
            }

            var vendaResultado = Venda.DeImovel(
                proposta.Id, proposta.ClienteId, imovel.ProprietarioId, proposta.ImovelId!.Value, proposta.AnuncioImovelId!.Value,
                proposta.CorretorId, proposta.UltimoValorNegociado, command.DataVenda, command.FormaPagamento, financiamento);

            if (!vendaResultado.IsSuccess)
                return Result<Guid>.Failure(vendaResultado.Error!);

            var venda = vendaResultado.Value!;
            vendaRepository.Adicionar(venda);
            await unitOfWork.SalvarAsync(cancellationToken);

            return Result<Guid>.Success(venda.Id);
        }
    }
}
