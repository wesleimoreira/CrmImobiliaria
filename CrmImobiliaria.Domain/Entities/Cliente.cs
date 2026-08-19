using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed class Cliente : AggregateRoot
    {
        private readonly HashSet<TipoCliente> _tipos = [];
        private readonly List<PerfilInteresse> _perfisInteresse = [];

        public string Nome { get; private set; }
        public CpfCnpj? Documento { get; private set; }
        public Telefone Telefone { get; private set; }
        public Telefone? WhatsApp { get; private set; }
        public Email Email { get; private set; }
        public IReadOnlySet<TipoCliente> Tipos => _tipos;
        public Guid CorretorResponsavelId { get; private set; }
        public OrigemCliente Origem { get; private set; }
        public string? CampanhaEspecifica { get; private set; }
        public DateTime? UltimoContato { get; private set; }
        public DateTime? ProximoContato { get; private set; }
        public string? Observacoes { get; private set; }
        public IReadOnlyList<PerfilInteresse> PerfisInteresse => _perfisInteresse;

        private Cliente(string nome, Telefone telefone, Email email, HashSet<TipoCliente> tipos, Guid corretorResponsavelId, OrigemCliente origem, CpfCnpj? documento, Telefone? whatsApp, string? campanhaEspecifica, string? observacoes)
        {
            Nome = nome;
            Telefone = telefone;
            Email = email;
            _tipos = tipos;
            CorretorResponsavelId = corretorResponsavelId;
            Origem = origem;
            Documento = documento;
            WhatsApp = whatsApp;
            CampanhaEspecifica = campanhaEspecifica;
            Observacoes = observacoes;
        }

        private Cliente()
        {
            Nome = null!;
            Telefone = null!;
            Email = null!;
        }

        public static Result<Cliente> Criar(string? nome, Telefone telefone, Email email, IEnumerable<TipoCliente> tipos, Guid corretorResponsavelId, OrigemCliente origem, CpfCnpj? documento = null, Telefone? whatsApp = null, string? campanhaEspecifica = null, string? observacoes = null)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return Result<Cliente>.Failure("Nome é obrigatório.");

            var tiposSet = tipos.ToHashSet();
            if (tiposSet.Count == 0)
                return Result<Cliente>.Failure("Cliente deve ter ao menos um tipo (Proprietário, Comprador, Locador, Locatário ou Investidor).");

            if (origem == OrigemCliente.CampanhaExterna && string.IsNullOrWhiteSpace(campanhaEspecifica))
                return Result<Cliente>.Failure("Informe a campanha específica quando a origem for Campanha Externa.");

            return Result<Cliente>.Success(new Cliente(nome.Trim(), telefone, email, tiposSet, corretorResponsavelId, origem, documento, whatsApp, campanhaEspecifica, observacoes));
        }

        public Result<bool> AdicionarPerfilInteresse(PerfilInteresse perfil)
        {
            var tipoNecessario = perfil.TipoNegociacao switch
            {
                TipoNegociacaoImovel.Venda => TipoCliente.Comprador,
                TipoNegociacaoImovel.Locacao => TipoCliente.Locatario,
                _ => (TipoCliente?)null
            };

            if (tipoNecessario is { } tipo && !_tipos.Contains(tipo))
                return Result<bool>.Failure($"Cliente precisa ser do tipo {tipo} para ter esse perfil de interesse.");

            _perfisInteresse.Add(perfil);
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public void RemoverPerfilInteresse(Guid perfilId)
        {
            _perfisInteresse.RemoveAll(p => p.Id == perfilId);
            MarcarAtualizado();
        }

        public Result<bool> AdicionarTipo(TipoCliente tipo)
        {
            _tipos.Add(tipo);
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> RemoverTipo(TipoCliente tipo)
        {
            if (_tipos.Count == 1 && _tipos.Contains(tipo))
                return Result<bool>.Failure("Cliente deve manter ao menos um tipo.");

            _tipos.Remove(tipo);
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public void DefinirDocumento(CpfCnpj documento)
        {
            Documento = documento;
            MarcarAtualizado();
        }

        public void AtualizarContato(Telefone telefone, Email email, Telefone? whatsApp = null)
        {
            Telefone = telefone;
            Email = email;
            WhatsApp = whatsApp;
            MarcarAtualizado();
        }

        public void RegistrarContato(DateTime dataContato, DateTime? proximoContato = null)
        {
            UltimoContato = dataContato;
            ProximoContato = proximoContato;
            MarcarAtualizado();
        }

        public void DefinirCorretorResponsavel(Guid corretorId)
        {
            CorretorResponsavelId = corretorId;
            MarcarAtualizado();
        }

        public void AtualizarObservacoes(string? observacoes)
        {
            Observacoes = observacoes;
            MarcarAtualizado();
        }
    }
}
