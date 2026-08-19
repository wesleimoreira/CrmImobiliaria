using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrmImobiliaria.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Documento = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    Telefone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Tipos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorretorResponsavelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Origem = table.Column<int>(type: "int", nullable: false),
                    CampanhaEspecifica = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UltimoContato = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProximoContato = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comissoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrigemTipo = table.Column<int>(type: "int", nullable: false),
                    OrigemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegraComissaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValorBase = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PercentualComissao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ComissaoTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ComissaoRecebida = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ComissaoDistribuida = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comissoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Corretores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Creci = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    GerenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Equipe = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corretores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empreendimentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LoteadoraIncorporadora = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Localizacao_Logradouro = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Localizacao_Numero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Localizacao_Complemento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Localizacao_Bairro = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Localizacao_Cidade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Localizacao_Uf = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Localizacao_Cep = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    DataLancamento = table.Column<DateOnly>(type: "date", nullable: true),
                    TotalLotes = table.Column<int>(type: "int", nullable: false),
                    NumeroQuadras = table.Column<int>(type: "int", nullable: false),
                    PercentualComissao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CampanhaVigente = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empreendimentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Imoveis",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProprietarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorretorCaptadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Endereco_Logradouro = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Endereco_Numero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Endereco_Complemento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Endereco_Bairro = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Endereco_Cidade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Endereco_Uf = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Endereco_Cep = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Area = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quartos = table.Column<int>(type: "int", nullable: false),
                    Suites = table.Column<int>(type: "int", nullable: false),
                    Garagem = table.Column<int>(type: "int", nullable: false),
                    Caracteristicas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fotos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Documentos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imoveis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leads",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorretorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EstagioAtual = table.Column<int>(type: "int", nullable: false),
                    EstagioAntesDePerda = table.Column<int>(type: "int", nullable: true),
                    MotivoPerda = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ImoveisApresentados = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProprietarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocatarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImovelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnuncioImovelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorretorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeadId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EstagioAtual = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ValorAluguel = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Periodo_DataInicial = table.Column<DateOnly>(type: "date", nullable: true),
                    Periodo_DataFinal = table.Column<DateOnly>(type: "date", nullable: true),
                    DiaVencimento = table.Column<int>(type: "int", nullable: true),
                    Garantia = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxaAdministracao = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IndiceReajuste = table.Column<int>(type: "int", nullable: true),
                    MotivoReprovacaoOuCancelamento = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmpreendimentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quadra = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Numero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Area = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EntradaMinima = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ParcelamentoMaximo = table.Column<int>(type: "int", nullable: false),
                    ValorPromocional = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PagamentosAluguel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocacaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Competencia = table.Column<int>(type: "int", nullable: false),
                    ValorDevido = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataVencimento = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DataPagamento = table.Column<DateOnly>(type: "date", nullable: true),
                    ValorPago = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagamentosAluguel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prestadores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Especialidade = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestadores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Propostas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImovelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AnuncioImovelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CorretorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeadId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ValorAnunciado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorProposto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Entrada = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NumeroParcelas = table.Column<int>(type: "int", nullable: true),
                    ValorParcela = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FormaPagamento = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    MotivoRecusa = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Propostas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegrasComissao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PercentualComissaoTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EmpreendimentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ImovelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegrasComissao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Repasses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocacaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PagamentoAluguelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Competencia = table.Column<int>(type: "int", nullable: false),
                    ValorAluguelRecebido = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxaAdministracao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorTaxaAdministracao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorLiquido = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DataRepasse = table.Column<DateOnly>(type: "date", nullable: true),
                    Comprovante = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReservasLote",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorretorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataReserva = table.Column<DateOnly>(type: "date", nullable: false),
                    DataExpiracao = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservasLote", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReservasTemporada",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImovelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnuncioImovelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Periodo_DataInicial = table.Column<DateOnly>(type: "date", nullable: false),
                    Periodo_DataFinal = table.Column<DateOnly>(type: "date", nullable: false),
                    ValorDiaria = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservasTemporada", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SolicitacoesManutencao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImovelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocacaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SolicitanteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    OrcamentoAprovadoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataConclusao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitacoesManutencao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vendas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PropostaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProprietarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ImovelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AnuncioImovelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CorretorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValorFinal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataVenda = table.Column<DateOnly>(type: "date", nullable: false),
                    FormaPagamento = table.Column<int>(type: "int", nullable: false),
                    Financiamento_Banco = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Financiamento_ValorFinanciado = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SituacaoDocumental = table.Column<int>(type: "int", nullable: false),
                    NumeroContrato = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UrlContrato = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    MotivoDistrato = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Visitas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImovelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnuncioImovelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorretorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeadId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Feedback = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vistorias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocacaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImovelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    DataAgendada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataRealizada = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResponsavelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Fotos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vistorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PerfisInteresse",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoNegociacao = table.Column<int>(type: "int", nullable: false),
                    TipoImovel = table.Column<int>(type: "int", nullable: false),
                    LocalizacaoDesejada = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FaixaPreco_Minimo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FaixaPreco_Maximo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NumeroQuartos = table.Column<int>(type: "int", nullable: true),
                    FormaPagamento = table.Column<int>(type: "int", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfisInteresse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerfisInteresse_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComissaoRateios",
                columns: table => new
                {
                    ComissaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Papel = table.Column<int>(type: "int", nullable: false),
                    Percentual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BeneficiarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComissaoRateios", x => new { x.ComissaoId, x.Id });
                    table.ForeignKey(
                        name: "FK_ComissaoRateios_Comissoes_ComissaoId",
                        column: x => x.ComissaoId,
                        principalTable: "Comissoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Anuncios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImovelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Exclusividade = table.Column<bool>(type: "bit", nullable: false),
                    RegraEstadia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DataFechamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anuncios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Anuncios_Imoveis_ImovelId",
                        column: x => x.ImovelId,
                        principalTable: "Imoveis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeadHistoricoEstagios",
                columns: table => new
                {
                    LeadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estagio = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DataHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadHistoricoEstagios", x => new { x.LeadId, x.Id });
                    table.ForeignKey(
                        name: "FK_LeadHistoricoEstagios_Leads_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Leads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropostaHistoricoNegociacoes",
                columns: table => new
                {
                    PropostaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Origem = table.Column<int>(type: "int", nullable: false),
                    DataHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaHistoricoNegociacoes", x => new { x.PropostaId, x.Id });
                    table.ForeignKey(
                        name: "FK_PropostaHistoricoNegociacoes_Propostas_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "Propostas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegraComissaoItensRateio",
                columns: table => new
                {
                    RegraComissaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Papel = table.Column<int>(type: "int", nullable: false),
                    Percentual = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegraComissaoItensRateio", x => new { x.RegraComissaoId, x.Id });
                    table.ForeignKey(
                        name: "FK_RegraComissaoItensRateio_RegrasComissao_RegraComissaoId",
                        column: x => x.RegraComissaoId,
                        principalTable: "RegrasComissao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RepasseDespesas",
                columns: table => new
                {
                    RepasseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepasseDespesas", x => new { x.RepasseId, x.Id });
                    table.ForeignKey(
                        name: "FK_RepasseDespesas_Repasses_RepasseId",
                        column: x => x.RepasseId,
                        principalTable: "Repasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orcamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrestadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SolicitacaoManutencaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orcamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orcamentos_SolicitacoesManutencao_SolicitacaoManutencaoId",
                        column: x => x.SolicitacaoManutencaoId,
                        principalTable: "SolicitacoesManutencao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Anuncios_Codigo",
                table: "Anuncios",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Anuncios_ImovelId",
                table: "Anuncios",
                column: "ImovelId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_Documento",
                table: "Clientes",
                column: "Documento",
                unique: true,
                filter: "[Documento] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Corretores_Creci",
                table: "Corretores",
                column: "Creci",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orcamentos_SolicitacaoManutencaoId",
                table: "Orcamentos",
                column: "SolicitacaoManutencaoId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfisInteresse_ClienteId",
                table: "PerfisInteresse",
                column: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Anuncios");

            migrationBuilder.DropTable(
                name: "ComissaoRateios");

            migrationBuilder.DropTable(
                name: "Corretores");

            migrationBuilder.DropTable(
                name: "Empreendimentos");

            migrationBuilder.DropTable(
                name: "LeadHistoricoEstagios");

            migrationBuilder.DropTable(
                name: "Locacoes");

            migrationBuilder.DropTable(
                name: "Lotes");

            migrationBuilder.DropTable(
                name: "Orcamentos");

            migrationBuilder.DropTable(
                name: "PagamentosAluguel");

            migrationBuilder.DropTable(
                name: "PerfisInteresse");

            migrationBuilder.DropTable(
                name: "Prestadores");

            migrationBuilder.DropTable(
                name: "PropostaHistoricoNegociacoes");

            migrationBuilder.DropTable(
                name: "RegraComissaoItensRateio");

            migrationBuilder.DropTable(
                name: "RepasseDespesas");

            migrationBuilder.DropTable(
                name: "ReservasLote");

            migrationBuilder.DropTable(
                name: "ReservasTemporada");

            migrationBuilder.DropTable(
                name: "Vendas");

            migrationBuilder.DropTable(
                name: "Visitas");

            migrationBuilder.DropTable(
                name: "Vistorias");

            migrationBuilder.DropTable(
                name: "Imoveis");

            migrationBuilder.DropTable(
                name: "Comissoes");

            migrationBuilder.DropTable(
                name: "Leads");

            migrationBuilder.DropTable(
                name: "SolicitacoesManutencao");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Propostas");

            migrationBuilder.DropTable(
                name: "RegrasComissao");

            migrationBuilder.DropTable(
                name: "Repasses");
        }
    }
}
