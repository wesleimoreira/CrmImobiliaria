using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed class Imovel : AggregateRoot
    {
        private readonly List<AnuncioImovel> _anuncios = [];
        private readonly List<string> _caracteristicas = [];
        private readonly List<string> _fotos = [];
        private readonly List<string> _documentos = [];

        public Guid ProprietarioId { get; private set; }
        public Guid CorretorCaptadorId { get; private set; }
        public TipoImovel Tipo { get; private set; }
        public Endereco Endereco { get; private set; }
        public Area Area { get; private set; }
        public int Quartos { get; private set; }
        public int Suites { get; private set; }
        public int Garagem { get; private set; }
        public IReadOnlyList<string> Caracteristicas => _caracteristicas;
        public IReadOnlyList<string> Fotos => _fotos;
        public IReadOnlyList<string> Documentos => _documentos;
        public IReadOnlyList<AnuncioImovel> Anuncios => _anuncios;

        private Imovel(Guid proprietarioId, Guid corretorCaptadorId, TipoImovel tipo, Endereco endereco, Area area, int quartos, int suites, int garagem)
        {
            ProprietarioId = proprietarioId;
            CorretorCaptadorId = corretorCaptadorId;
            Tipo = tipo;
            Endereco = endereco;
            Area = area;
            Quartos = quartos;
            Suites = suites;
            Garagem = garagem;
        }

        private Imovel()
        {
            Endereco = null!;
            Area = null!;
        }

        public static Result<Imovel> Criar(Guid proprietarioId, Guid corretorCaptadorId, TipoImovel tipo, Endereco endereco, Area area, int quartos = 0, int suites = 0, int garagem = 0)
        {
            if (quartos < 0 || suites < 0 || garagem < 0)
                return Result<Imovel>.Failure("Quartos, suítes e vagas de garagem não podem ser negativos.");

            if (suites > quartos)
                return Result<Imovel>.Failure("Número de suítes não pode ser maior que o de quartos.");

            return Result<Imovel>.Success(new Imovel(proprietarioId, corretorCaptadorId, tipo, endereco, area, quartos, suites, garagem));
        }

        public Result<AnuncioImovel> AdicionarAnuncio(TipoNegociacaoImovel tipo, CodigoImovel codigo, Dinheiro valor, bool exclusividade = false, RegraEstadia? regraEstadia = null)
        {
            if (_anuncios.Any(a => a.Tipo == tipo && a.EstaAtivo))
                return Result<AnuncioImovel>.Failure($"Já existe um anúncio ativo do tipo {tipo} para este imóvel.");

            var anuncioResult = AnuncioImovel.Criar(Id, tipo, codigo, valor, exclusividade, regraEstadia);
            if (!anuncioResult.IsSuccess)
                return Result<AnuncioImovel>.Failure(anuncioResult.Error!);

            _anuncios.Add(anuncioResult.Value!);
            MarcarAtualizado();
            return Result<AnuncioImovel>.Success(anuncioResult.Value!);
        }

        public Result<bool> RemoverAnuncio(Guid anuncioId)
        {
            var anuncio = _anuncios.FirstOrDefault(a => a.Id == anuncioId);
            if (anuncio is null)
                return Result<bool>.Failure("Anúncio não encontrado.");

            if (anuncio.Status is StatusAnuncio.Vendido or StatusAnuncio.Alugado)
                return Result<bool>.Failure("Não é possível remover um anúncio já concluído — mantenha para histórico.");

            _anuncios.Remove(anuncio);
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public void AdicionarCaracteristica(string caracteristica)
        {
            if (string.IsNullOrWhiteSpace(caracteristica)) return;
            if (_caracteristicas.Contains(caracteristica, StringComparer.OrdinalIgnoreCase)) return;

            _caracteristicas.Add(caracteristica.Trim());
            MarcarAtualizado();
        }

        public void RemoverCaracteristica(string caracteristica)
        {
            _caracteristicas.RemoveAll(c => c.Equals(caracteristica, StringComparison.OrdinalIgnoreCase));
            MarcarAtualizado();
        }

        public void AdicionarFoto(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return;
            _fotos.Add(url.Trim());
            MarcarAtualizado();
        }

        public void RemoverFoto(string url)
        {
            _fotos.Remove(url);
            MarcarAtualizado();
        }

        public void AdicionarDocumento(string urlOuNome)
        {
            if (string.IsNullOrWhiteSpace(urlOuNome)) return;
            _documentos.Add(urlOuNome.Trim());
            MarcarAtualizado();
        }

        public void AtualizarCaracteristicasFisicas(int quartos, int suites, int garagem)
        {
            Quartos = quartos;
            Suites = suites;
            Garagem = garagem;
            MarcarAtualizado();
        }

        public void AtualizarEndereco(Endereco endereco)
        {
            Endereco = endereco;
            MarcarAtualizado();
        }

        public void DefinirCorretorCaptador(Guid corretorId)
        {
            CorretorCaptadorId = corretorId;
            MarcarAtualizado();
        }
    }
}
