﻿using System.Collections;
using System.ComponentModel;

namespace KiDev.DataStructures;

/// <summary>
/// Represents a strongly typed list of objects that is very fast to aggregate.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public class SegmentTree<T> : IAggregableList<T>, IReadOnlyAggregableList<T>, IList
{
    private const string ST_IsFixedSize = "Size of SegmentTree cannot be changed. This method will throw a NotSupportedException.";

    private readonly Func<T, T, T> _aggregator;
    private readonly T[] _storage;
    private readonly int _start, _length;
    private readonly T _basis;

    /// <summary>
    /// Initializes a new instance of the <see cref="SegmentTree{T}"/> class that has specified length and filled with <paramref name="basis"/>,
    /// optionally copying values from <paramref name="original"/> source.
    /// </summary>
    /// <param name="length">The number of elements that the new segment tree can store.</param>
    /// <param name="aggregator">
    /// Function used to aggregate elements.<br/>
    /// <b>THIS FUNCTION MUST MEET SEVERAL CONDITIONS, see remarks.</b>
    /// </param>
    /// <param name="original">Source to copy values from.</param>
    /// <param name="basis">The value that is initial for the elements and neutral for the aggregator function.</param>
    /// <remarks>
    /// Actual memory consumption is O(2*P), where P is the smallest power of two not smaller than <paramref name="length"/>
    /// <br/><br/>
    /// <paramref name="aggregator"/> function should meet these conditions 
    /// ('<c>G</c>' is <paramref name="aggregator"/>, '<c>B</c>' is <paramref name="basis"/>, 
    /// '<c>E</c>', '<c>F</c>' and '<c>H</c>' are arbitrary instances of <typeparamref name="T"/> among the assumed range): <br/>
    /// <code>
    /// G(B, E) == E
    /// G(E, F) == G(F, E)
    /// G(E, G(F, H)) == G(F, G(E, H))
    /// </code>
    /// <b>If these conditions are violated, the BEHAVIOR of the <see cref="Aggregate(Range)"/> method is UNDEFINED.</b>
    /// </remarks>
    public SegmentTree(int length, Func<T, T, T> aggregator, IEnumerable<T>? original = null, T basis = default!)
    {
        _basis = basis;
        _length = length;
        _aggregator = aggregator;

        var p = MinPow2(length);

        _storage = new T[1 << (p+1)];
        _start = _storage.Length / 2;
        if(original is null)
            Clear();
        else
        {
            int index = 0;
            var enumr = original.GetEnumerator();
            while(index < _length && enumr.MoveNext())
                _storage[_start + index++] = enumr.Current;
            while (index < _length)
                _storage[_start + index++] = basis;
        }
    }

    /// <inheritdoc/>
    public int Count => _length;

    /// <inheritdoc/>
    public bool IsReadOnly => false;

    /// <inheritdoc/>
    public bool IsFixedSize => true;

    /// <inheritdoc/>
    public bool IsSynchronized => false;

    /// <inheritdoc/>
    public object SyncRoot => this;

    /// <inheritdoc/>
    public T Aggregate(Range at)
    {
        var (left, len) = at.GetOffsetAndLength(_length);
        left += _start;
        var right = left + len - 1;

        var ret = _basis;
        while (left <= right)
        {
            if ((left % 2) == 1) ret = _aggregator(ret, _storage[left]);
            if ((right % 2) == 0) ret = _aggregator(ret, _storage[right]);

            left = (left + 1) / 2;
            right = (right - 1) / 2;
        }
        return ret;
    }

    /// <inheritdoc/>
    public void Clear()
    {
#if NETSTANDARD2_1_OR_GREATER
        Array.Fill(_storage, _basis);
#else
        for(var i = 0; i < _storage.Length; i++) _storage[i] = _basis;
#endif
    }

    /// <inheritdoc/>
    public void CopyTo(Array array, int index)
    {
        if (array is not T[] arr) 
            throw new ArgumentException($"Wrong type of array {array.GetType().Name}, expected {typeof(T[]).Name}");
        CopyTo(arr, index);
    }

    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex) => Array.Copy(_storage, _start, array, arrayIndex, _length);

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => _storage.Skip(_start).GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #region Indexation
    /// <inheritdoc/>
    object IList.this[int index]
    {
        get => this[index]!;
        set => this[index] = (T)value;
    }

    /// <inheritdoc/>
    public T this[Index index]
    {
        get => this[index.GetOffset(_length)];
        set => this[index.GetOffset(_length)] = value;
    }

    /// <inheritdoc/>
    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= _length) throw new IndexOutOfRangeException(nameof(index));
            return _storage[index + _start];
        }
        set
        {
            if (index < 0 || index >= _length) throw new IndexOutOfRangeException(nameof(index));
            index += _start;
            _storage[index] = value;
            index /= 2;
            while (index > 0)
            {
                _storage[index] = _aggregator(_storage[index * 2], _storage[index * 2 + 1]);
                index /= 2;
            }
        }
    }
    #endregion

    #region Searching
    /// <inheritdoc/>
    public bool Contains(object value) => value is T t && Contains(t);

    /// <inheritdoc/>
    public int IndexOf(object value) => value is T t ? IndexOf(t) : -1;

    /// <inheritdoc/>
    public bool Contains(T item) => _length != 0 && IndexOf(item) > -1;

    /// <inheritdoc/>
    public int IndexOf(T item) => IndexOf(item, 0);

    /// <inheritdoc/>
    public int IndexOf(T item, int startIndex)
    {
        if (startIndex < 0 || startIndex >= _length) 
            throw new ArgumentOutOfRangeException(nameof(startIndex));

        var index = Array.IndexOf(_storage, item, _start + startIndex);
        if (index < _start) return -1;
        return index - _start;
    }
    #endregion

    #region Not supported
    [Obsolete(ST_IsFixedSize)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Add(T item) => throw new NotSupportedException();

    [Obsolete(ST_IsFixedSize)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public int Add(object value) => throw new NotSupportedException();

    [Obsolete(ST_IsFixedSize)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Insert(int index, T item) => throw new NotSupportedException();

    [Obsolete(ST_IsFixedSize)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Insert(int index, object value) => throw new NotSupportedException();

    [Obsolete(ST_IsFixedSize)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool Remove(T item) => throw new NotSupportedException();

    [Obsolete(ST_IsFixedSize)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Remove(object value) => throw new NotSupportedException();

    [Obsolete(ST_IsFixedSize)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void RemoveAt(int index) => throw new NotSupportedException();
    #endregion

    // Returns mininal power of two not smaller than n.
    private static int MinPow2(int n)
    {
        int p = 0, a = 1;
        while (a < n)
        {
            a *= 2;
            p++;
        }
        return p;
    }
}