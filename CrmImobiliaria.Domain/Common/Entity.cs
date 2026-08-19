namespace CrmImobiliaria.Domain.Common
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
        public DateTime CriadoEm { get; protected set; }
        public DateTime? AtualizadoEm { get; protected set; }

        protected Entity()
        {
            Id = Guid.CreateVersion7();
            CriadoEm = DateTime.UtcNow;
        }

        protected Entity(Guid id)
        {
            Id = id;
            CriadoEm = DateTime.UtcNow;
        }

        protected void MarcarAtualizado() => AtualizadoEm = DateTime.UtcNow;

        public override bool Equals(object? obj)
        {
            if (obj is not Entity other) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            return Id == other.Id;
        }

        public override int GetHashCode() => HashCode.Combine(GetType(), Id);

        public static bool operator ==(Entity? left, Entity? right) => left is null ? right is null : left.Equals(right);

        public static bool operator !=(Entity? left, Entity? right) => !(left == right);
    }
}
