using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Locacoes.Commands
{
    public enum AcaoLocacao
    {
        RegistrarVisita,
        IniciarAnaliseCadastral,
        Aprovar,
        Reprovar,
        RegistrarVistoriaEntrada,
        EntregarChaves,
        Encerrar,
        Cancelar
    }

    public sealed record AvancarEstagioLocacaoCommand(Guid Id, AcaoLocacao Acao, string? Texto) : ICommand<bool>;
}
