using CrmImobiliaria.Domain.ValueObjects;

namespace CrmImobiliaria.Web.Formatting
{
    // Formata os campos assim que o usuário sai deles (on blur), reaproveitando o Formatado
    // dos próprios Value Objects do Domain — nunca duplica a lógica de validação/formatação aqui.
    // Não usamos o MudMask/PatternMask nativo do MudBlazor: tem bug conhecido e sem correção de
    // cursor pulando em Blazor Server (só funciona bem em WASM). Se o valor não é válido ainda,
    // devolve como veio — o erro final aparece no Result.Error ao salvar, igual já era.
    public static class Mascaras
    {
        public static string? FormatarTelefone(string? valor) =>
            !string.IsNullOrWhiteSpace(valor) && Telefone.Criar(valor) is { IsSuccess: true, Value: not null } resultado
                ? resultado.Value.Formatado
                : valor;

        public static string? FormatarCpfCnpj(string? valor) =>
            !string.IsNullOrWhiteSpace(valor) && CpfCnpj.Criar(valor) is { IsSuccess: true, Value: not null } resultado
                ? resultado.Value.Formatado
                : valor;
    }
}
