namespace CrmImobiliaria.Domain.Common
{
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        protected abstract IEnumerable<object?> GetEqualityComponents();

        public override bool Equals(object? obj)
        {
            if (obj is not ValueObject other || GetType() != other.GetType())
                return false;

            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public bool Equals(ValueObject? other) => Equals((object?)other);

        public override int GetHashCode() => GetEqualityComponents().Select(x => x?.GetHashCode() ?? 0).Aggregate((x, y) => x ^ y);

        public static bool operator ==(ValueObject? left, ValueObject? right) => left is null ? right is null : left.Equals(right);

        public static bool operator !=(ValueObject? left, ValueObject? right) => !(left == right);
    }
}
