using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed class Empreendimento : AggregateRoot
    {
        public string Nome { get; private set; }
        public string LoteadoraIncorporadora { get; private set; }
        public Endereco Localizacao { get; private set; }
        public DateOnly? DataLancamento { get; private set; }
        public int TotalLotes { get; private set; }
        public int NumeroQuadras { get; private set; }
        public Percentual PercentualComissao { get; private set; }
        public string? CampanhaVigente { get; private set; }
        public StatusEmpreendimento Status { get; private set; }

        private Empreendimento(string nome, string loteadoraIncorporadora, Endereco localizacao, int totalLotes, int numeroQuadras, Percentual percentualComissao, DateOnly? dataLancamento)
        {
            Nome = nome;
            LoteadoraIncorporadora = loteadoraIncorporadora;
            Localizacao = localizacao;
            TotalLotes = totalLotes;
            NumeroQuadras = numeroQuadras;
            PercentualComissao = percentualComissao;
            DataLancamento = dataLancamento;
            Status = StatusEmpreendimento.EmCaptacao;
        }

        private Empreendimento()
        {
            Nome = null!;
            LoteadoraIncorporadora = null!;
            Localizacao = null!;
            PercentualComissao = null!;
        }

        public static Result<Empreendimento> Criar(string? nome, string? loteadoraIncorporadora, Endereco localizacao, int totalLotes, int numeroQuadras, Percentual percentualComissao, DateOnly? dataLancamento = null)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return Result<Empreendimento>.Failure("Nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(loteadoraIncorporadora))
                return Result<Empreendimento>.Failure("Loteadora/incorporadora é obrigatória.");

            if (totalLotes <= 0 || numeroQuadras <= 0)
                return Result<Empreendimento>.Failure("Total de lotes e número de quadras devem ser maiores que zero.");

            return Result<Empreendimento>.Success(new Empreendimento(
                nome.Trim(), loteadoraIncorporadora.Trim(), localizacao, totalLotes, numeroQuadras, percentualComissao, dataLancamento));
        }

        public void DefinirCampanha(string? campanha)
        {
            CampanhaVigente = campanha;
            MarcarAtualizado();
        }

        public Result<bool> Lancar(DateOnly dataLancamento)
        {
            if (Status != StatusEmpreendimento.EmCaptacao)
                return Result<bool>.Failure("Só é possível lançar um empreendimento em captação.");

            Status = StatusEmpreendimento.Lancado;
            DataLancamento = dataLancamento;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> IniciarComercializacao()
        {
            if (Status != StatusEmpreendimento.Lancado)
                return Result<bool>.Failure("Só é possível comercializar um empreendimento lançado.");

            Status = StatusEmpreendimento.EmComercializacao;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> Suspender()
        {
            if (Status is StatusEmpreendimento.Encerrado)
                return Result<bool>.Failure("Não é possível suspender um empreendimento encerrado.");

            Status = StatusEmpreendimento.Suspenso;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> Encerrar()
        {
            Status = StatusEmpreendimento.Encerrado;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }
    }
}
