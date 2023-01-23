namespace KiDev.DataStructures;

/// <summary>
/// Array-based implementation of <see cref="IDisjointSetUnion{T, DisjointSetUnionDictionary{T}.SetHandle}"/>
/// that can combine items into sets with O(1) complexity. The items are integers of a fixed range.
/// </summary>
/// <typeparam name="T">The type of items in the sets.</typeparam>
public class DisjointSetUnionIndexOnly : IDisjointSetUnion<int, DisjointSetUnionIndexOnly.SetHandle>,
                                         IDisjointSetUnion<Index, DisjointSetUnionIndexOnly.SetHandle>
{
    private readonly int[] _array;

    public DisjointSetUnionIndexOnly(int length)
    {
        _array = new int[length];
        for (var i = 0; i < length; i++) _array[i] = i;
    }

    private int GetLeader(int value)
    {
        var parent = _array[value];
        if (value == parent) return parent;
        return _array[value] = GetLeader(parent);
    }


    /// <inheritdoc/>
    public bool InSameSet(Index a, Index b) => InSameSet(a.GetOffset(_array.Length), b.GetOffset(_array.Length));

    /// <inheritdoc/>
    public bool InSameSet(int a, int b)
    {
        Helpers.CheckBounds(a, 0, _array.Length, nameof(a));
        Helpers.CheckBounds(b, 0, _array.Length, nameof(b));
        return GetLeader(a) == GetLeader(b);
    }


    /// <inheritdoc/>
    public SetHandle SetOf(Index a) => SetOf(a.GetOffset(_array.Length));

    /// <inheritdoc/>
    public SetHandle SetOf(int a)
    {
        Helpers.CheckBounds(a, 0, _array.Length, nameof(a));
        return new(this, a);
    }


    /// <inheritdoc/>
    public bool Unite(Index a, Index b) => Unite(a.GetOffset(_array.Length), b.GetOffset(_array.Length));

    /// <inheritdoc/>
    public bool Unite(int a, int b)
    {
        Helpers.CheckBounds(a, 0, _array.Length, nameof(a));
        Helpers.CheckBounds(b, 0, _array.Length, nameof(b));
        (a, b) = (GetLeader(a), GetLeader(b));
        if (a == b) return false;
        _array[b] = a;
        return true;
    }

    /// <inheritdoc/>
    public IReadOnlyDictionary<SetHandle, IEnumerable<int>> Group(IEnumerable<int> elements) =>
        Helpers.DefaultGroup<SetHandle, int>(this, elements);

    /// <inheritdoc/>
    public IReadOnlyDictionary<SetHandle, IEnumerable<Index>> Group(IEnumerable<Index> elements) =>
        Helpers.DefaultGroup<SetHandle, Index>(this, elements);

    /// <summary>
    /// Represents a set handle in <see cref="DisjointSetUnionIndexOnly"/>.
    /// </summary>
    public struct SetHandle
    {
        private readonly DisjointSetUnionIndexOnly _container;
        private int _element;

        internal SetHandle(DisjointSetUnionIndexOnly container, int element)
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
            return leadA == leadB;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is not SetHandle hndl) return false;
            return this == hndl;
        }

        /// <inheritdoc/>
        public override int GetHashCode() => _container.GetLeader(_element)!.GetHashCode();
    }
}
