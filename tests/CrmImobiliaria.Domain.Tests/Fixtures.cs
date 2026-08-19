using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Domain.ValueObjects;

namespace CrmImobiliaria.Domain.Tests
{
    // Atalhos para criar Value Objects válidos nos testes sem repetir ".Value!" em todo lugar.
    internal static class Fixtures
    {
        public static Dinheiro CriarDinheiro(decimal valor) => Dinheiro.CriarPositivo(valor).Value!;
        public static Telefone CriarTelefone(string numero = "11987654321") => Telefone.Criar(numero).Value!;
        public static Email CriarEmail(string endereco = "teste@crm.com") => Email.Criar(endereco).Value!;
        public static Percentual CriarPercentual(decimal valor) => Percentual.Criar(valor).Value!;
        public static Area CriarArea(decimal metrosQuadrados = 100) => Area.Criar(metrosQuadrados).Value!;

        public static CodigoImovel CriarCodigo(TipoNegociacaoImovel tipo, int sequencial = 1) =>
            CodigoImovel.Criar(tipo, 2026, sequencial).Value!;
    }
}
