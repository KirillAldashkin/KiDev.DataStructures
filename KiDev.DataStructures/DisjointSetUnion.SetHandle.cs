namespace KiDev.DataStructures;

public partial class DisjointSetUnionDictionary<T>
{
    /// <summary>
    /// Represents a set handle in <see cref="DisjointSetUnionDictionary{T}"/>.
    /// </summary>
    public struct SetHandle
    {
        private readonly DisjointSetUnionDictionary<T> _container;
        private T _element;

        internal SetHandle(DisjointSetUnionDictionary<T> container, T element)
        {
            _container = container;
            _element = element;
        }

        public static bool operator !=(SetHandle a, SetHandle b) => !(a == b);

        public static bool operator ==(SetHandle a, SetHandle b)
        {
            if (a._container != b._container) return false;
            var leadA = a._container.GetLeader(a._element);
            var leadB = b._container.GetLeader(b._element);
            return a._container._same(leadA, leadB);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is not SetHandle hndl) return false;
            return this == hndl;
        }

        /// <inheritdoc/>
        public override int GetHashCode() => _container.GetLeader(_element)!.GetHashCode();
    }
}
