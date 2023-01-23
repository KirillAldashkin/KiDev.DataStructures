#if NET6_0_OR_GREATER
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#endif

namespace KiDev.DataStructures;

/// <summary>
/// Dictionary-based implementation of <see cref="IDisjointSetUnion{T, DisjointSetUnionDictionary{T}.SetHandle}"/>
/// that can combine items into sets with O(1) complexity.
/// </summary>
/// <typeparam name="T">The type of items in the sets.</typeparam>
public partial class DisjointSetUnionDictionary<T> : IDisjointSetUnion<T, DisjointSetUnionDictionary<T>.SetHandle>
{
    // The main collection that is used to resolve sets of items. The key is an item,
    // and the value is another item, or is equal to key if item is the set leader.
    // A set leader is an element that describes the set in which it is contained.
    private Dictionary<T, T> _storage;
    private readonly Func<T, T, bool> _same;

    /// <summary>
    /// Creates a new instance of <see cref="DisjointSetUnionDictionary{T}"/>, in which each element is in a 
    /// separate set, using a given function to compare instances of <typeparamref name="T"/>.
    /// </summary>
    /// <param name="capacity">
    /// The initial number of elements that the <see cref="Dis"/>> can contain.
    /// </param>
    /// <param name="same">
    /// Function used to determine if two <typeparamref name="T"/> instances are equals.
    /// Default is <see cref="EqualityComparer{T}.Equals(T, T)"/> of <see cref="EqualityComparer{T}.Default"/>.
    /// </param>
    public DisjointSetUnionDictionary(int capacity = 0, Func<T, T, bool>? same = null)
    {
        _same = same ?? EqualityComparer<T>.Default.Equals;
        _storage = new(capacity);
    }

#if NET6_0_OR_GREATER
    // Returns the leader of the set in which given element is located.
    // 'inDict' tells whether returned reference is located in '_storage'
    private ref T GetLeaderRef(ref T item, out bool inDict)
    {
        ref var parent = ref CollectionsMarshal.GetValueRefOrNullRef(_storage, item);
        if (Unsafe.IsNullRef(ref parent))
        {
            inDict = false;
            return ref item;
        }
        if (_same(item, parent))
        {
            inDict = true;
            return ref parent;
        }

        parent = GetLeaderRef(ref parent, out inDict);
        return ref parent;
    }
#endif

    // Returns the leader of the set in which given element is located.
    private T GetLeader(T item)
    {
#if NET6_0_OR_GREATER
        return GetLeaderRef(ref item, out _);
#else
        if (!_storage.TryGetValue(item, out var parent)) return item;
        if (_same(item, parent)) return parent;

        return _storage[item] = GetLeader(parent);
#endif
    }

    /// <inheritdoc/>
    public bool InSameSet(T a, T b)
    {
#if NET6_0_OR_GREATER
        ref var ra = ref GetLeaderRef(ref a, out var inDictA);
        ref var rb = ref GetLeaderRef(ref b, out var inDictB);
        // If both references are from dictionary, then we can just
        // compare addresess instead of using '_same' method.
        if(inDictA && inDictB) return Unsafe.AreSame(ref ra, ref rb);
        return _same(ra, rb);
#else
        return _same(GetLeader(a), GetLeader(b));
#endif
    }

    /// <inheritdoc/>
    public bool Unite(T a, T b)
    {
#if NET6_0_OR_GREATER
        ref var ra = ref GetLeaderRef(ref a, out var inDictA);
        ref var rb = ref GetLeaderRef(ref b, out var inDictB);
        if (inDictA && inDictB)
        {
            if (Unsafe.AreSame(ref ra, ref rb)) return false;
        }
        else if (_same(ra, rb)) return false;

        // Use direct reference to the internals of _storage if possible
        if (inDictB) 
            rb = ra;
        else
            _storage[rb] = ra;
        return true;
#else
        (a, b) = (GetLeader(a), GetLeader(b));
        if (_same(a, b)) return false;
        _storage[b] = a;
        return true;
#endif
    }

    /// <inheritdoc/>
    public DisjointSetUnionDictionary<T>.SetHandle SetOf(T a) => new(this, a);

    /// <inheritdoc/>
    public IReadOnlyDictionary<DisjointSetUnionDictionary<T>.SetHandle, IEnumerable<T>> Group(IEnumerable<T> elements) =>
        Helpers.DefaultGroup(this, elements);
}
