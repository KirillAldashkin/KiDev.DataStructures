namespace KiDev.DataStructures;

/// <summary>
/// Represents a container of items that can combine them into sets with O(1) complexity.
/// </summary>
/// <typeparam name="T">The type of items in the sets.</typeparam>
public partial class DisjointSetUnion<T> : IDisjointSetUnion<T, DisjointSetUnion<T>.SetHandle>
{
    private Dictionary<T, T> _storage = new();
    private readonly Func<T, T, bool> _same;

    /// <summary>
    /// Creates a new instance of <see cref="DisjointSetUnion{T}"/>, in which each element is in a 
    /// separate set, using a given function to compare instances of <typeparamref name="T"/>.
    /// </summary>
    /// <param name="same">
    /// Function used to determine if two <typeparamref name="T"/> instances are equals.
    /// Default is <see cref="EqualityComparer{T}.Equals(T, T)"/> of <see cref="EqualityComparer{T}.Default"/>.
    /// </param>
    public DisjointSetUnion(Func<T, T, bool>? same = null) =>
        _same = same ?? EqualityComparer<T>.Default.Equals;

    // Returns the leader of the set in which given element is located.
    // The leader of a set is an element that describes this set.
    private T GetLeader(T item)
    {
        if (!_storage.TryGetValue(item, out var parent)) return item;
        if (_same(item, parent)) return parent;

        return _storage[item] = GetLeader(parent);
    }

    /// <inheritdoc/>
    public bool InSameSet(T a, T b) => _same(GetLeader(a), GetLeader(b));

    /// <inheritdoc/>
    public bool Unite(T a, T b)
    {
        (a, b) = (GetLeader(a), GetLeader(b));
        if (_same(a, b)) return false;
        _storage[b] = a;
        return true;
    }

    /// <inheritdoc/>
    public DisjointSetUnion<T>.SetHandle SetOf(T a) => new(this, a);

    /// <inheritdoc/>
    public IReadOnlyDictionary<SetHandle, IEnumerable<T>> Group(IEnumerable<T> elements)
    {
        var dict = new Dictionary<SetHandle, List<T>>();
        foreach(var item in elements)
        {
            var set = SetOf(item);
            if (!dict.TryGetValue(set, out var list)) 
            {
                list = new(); 
                dict[set] = list;
            }
            list.Add(item);
        }
        return (IReadOnlyDictionary<SetHandle, IEnumerable<T>>)dict;
    }
}
