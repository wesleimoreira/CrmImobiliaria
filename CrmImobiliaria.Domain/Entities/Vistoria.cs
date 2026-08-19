using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed class Vistoria : AggregateRoot
    {
        private readonly List<string> _fotos = [];

        public Guid LocacaoId { get; private set; }
        public Guid ImovelId { get; private set; }
        public TipoVistoria Tipo { get; private set; }
        public DateTime DataAgendada { get; private set; }
        public DateTime? DataRealizada { get; private set; }
        public Guid ResponsavelId { get; private set; }
        public StatusVistoria Status { get; private set; }
        public string? Observacoes { get; private set; }
        public IReadOnlyList<string> Fotos => _fotos;

        private Vistoria(Guid locacaoId, Guid imovelId, TipoVistoria tipo, DateTime dataAgendada, Guid responsavelId)
        {
            LocacaoId = locacaoId;
            ImovelId = imovelId;
            Tipo = tipo;
            DataAgendada = dataAgendada;
            ResponsavelId = responsavelId;
            Status = StatusVistoria.Agendada;
        }

        private Vistoria() { }

        public static Result<Vistoria> Agendar(Guid locacaoId, Guid imovelId, TipoVistoria tipo, DateTime dataAgendada, Guid responsavelId) => Result<Vistoria>.Success(new Vistoria(locacaoId, imovelId, tipo, dataAgendada, responsavelId));

        public Result<bool> RegistrarRealizacao(DateTime dataRealizada, string? observacoes = null)
        {
            if (Status != StatusVistoria.Agendada)
                return Result<bool>.Failure("Só é possível registrar realização de uma vistoria agendada.");

            Status = StatusVistoria.Realizada;
            DataRealizada = dataRealizada;
            Observacoes = observacoes;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public void AdicionarFoto(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return;

            _fotos.Add(url.Trim());
            MarcarAtualizado();
        }

        public Result<bool> Cancelar(string? motivo = null)
        {
            if (Status == StatusVistoria.Realizada)
                return Result<bool>.Failure("Não é possível cancelar uma vistoria já realizada.");

            Status = StatusVistoria.Cancelada;
            Observacoes = motivo;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }
    }
}
