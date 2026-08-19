using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Empreendimentos.Commands
{
    public enum AcaoEmpreendimento { Lancar, IniciarComercializacao, Suspender, Encerrar }

    public sealed record AvancarStatusEmpreendimentoCommand(Guid Id, AcaoEmpreendimento Acao, DateOnly? DataLancamento = null) : ICommand<bool>;
}
