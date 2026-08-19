using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed class Corretor : AggregateRoot
    {
        public string Nome { get; private set; }
        public Creci Creci { get; private set; }
        public Telefone Telefone { get; private set; }
        public Email Email { get; private set; }
        public Guid? GerenteId { get; private set; }
        public string? Equipe { get; private set; }
        public StatusCorretor Status { get; private set; }

        private Corretor(string nome, Creci creci, Telefone telefone, Email email, Guid? gerenteId, string? equipe)
        {
            Nome = nome;
            Creci = creci;
            Telefone = telefone;
            Email = email;
            GerenteId = gerenteId;
            Equipe = equipe;
            Status = StatusCorretor.Ativo;
        }

        // Necessário para o EF Core materializar a entidade — nunca use diretamente no código de negócio
        private Corretor()
        {
            Nome = null!;
            Creci = null!;
            Telefone = null!;
            Email = null!;
        }

        public static Result<Corretor> Criar(string? nome, Creci creci, Telefone telefone, Email email, Guid? gerenteId = null, string? equipe = null)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return Result<Corretor>.Failure("Nome é obrigatório.");

            return Result<Corretor>.Success(new Corretor(nome.Trim(), creci, telefone, email, gerenteId, equipe));
        }

        public void AtualizarContato(Telefone telefone, Email email)
        {
            Telefone = telefone;
            Email = email;
            MarcarAtualizado();
        }

        public Result<bool> DefinirGerente(Guid gerenteId)
        {
            if (gerenteId == Id)
                return Result<bool>.Failure("Um corretor não pode ser gerente de si mesmo.");

            GerenteId = gerenteId;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public void RemoverGerente()
        {
            GerenteId = null;
            MarcarAtualizado();
        }

        public void DefinirEquipe(string? equipe)
        {
            Equipe = equipe;
            MarcarAtualizado();
        }

        public void Inativar()
        {
            Status = StatusCorretor.Inativo;
            MarcarAtualizado();
        }

        public void Suspender()
        {
            Status = StatusCorretor.Suspenso;
            MarcarAtualizado();
        }

        public void Reativar()
        {
            Status = StatusCorretor.Ativo;
            MarcarAtualizado();
        }
    }
}
