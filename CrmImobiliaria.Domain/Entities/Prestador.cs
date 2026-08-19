using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed class Prestador : AggregateRoot
    {
        public string Nome { get; private set; }
        public Telefone Telefone { get; private set; }
        public Email? Email { get; private set; }
        public string? Especialidade { get; private set; }
        public bool Ativo { get; private set; }

        private Prestador(string nome, Telefone telefone, Email? email, string? especialidade)
        {
            Nome = nome;
            Telefone = telefone;
            Email = email;
            Especialidade = especialidade;
            Ativo = true;
        }

        private Prestador()
        {
            Nome = null!;
            Telefone = null!;
        }

        public static Result<Prestador> Criar(string? nome, Telefone telefone, Email? email = null, string? especialidade = null)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return Result<Prestador>.Failure("Nome é obrigatório.");

            return Result<Prestador>.Success(new Prestador(nome.Trim(), telefone, email, especialidade));
        }

        public void AtualizarContato(Telefone telefone, Email? email)
        {
            Telefone = telefone;
            Email = email;
            MarcarAtualizado();
        }

        public void Inativar()
        {
            Ativo = false;
            MarcarAtualizado();
        }

        public void Reativar()
        {
            Ativo = true;
            MarcarAtualizado();
        }
    }
}
