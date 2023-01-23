using System.Collections;

namespace KiDev.DataStructures;

internal class AggregableSubList<T> : IAggregableList<T>, IReadOnlyAggregableList<T>, IList
{
    private readonly IAggregableList<T> _from;
    private readonly int _start, _length;

    public AggregableSubList(IAggregableList<T> from, int start, int length)
    {
        if (length < 0) 
            throw new ArgumentOutOfRangeException(nameof(length));
        if(start < 0 || start + length > from.Count)
            throw new ArgumentOutOfRangeException(nameof(start));

        _from = from;
        _start = start;
        _length = length;
    }

#pragma warning disable 8769, 8603 // implementation of old IList
    object IList.this[int index]
    { 
        get => this[index];
        set => this[index] = (T)value;
    }
#pragma warning restore 8769, 8603

    public T this[Index index] 
    { 
        get => this[index.GetOffset(_length)]; 
        set => this[index.GetOffset(_length)] = value;
    }
    public T this[int index] 
    {
        get
        {
            if (index < 0 || index >= _length) throw new IndexOutOfRangeException(nameof(index));
            return _from[index + _start];
        }
        set
        {
            if (index < 0 || index >= _length) throw new IndexOutOfRangeException(nameof(index));
            _from[index + _start] = value;
        }
    }

    public int Count => _length;
    public bool IsReadOnly => _from.IsReadOnly;
    public bool IsFixedSize => true;
    public bool IsSynchronized => (_from is ICollection coll) && coll.IsSynchronized;
    public object SyncRoot => (_from is ICollection coll) ? coll.SyncRoot : _from;

    public T Aggregate(Range at)
    {
        var (start, len) = at.GetOffsetAndLength(_length);
        if (start < 0) throw new ArgumentOutOfRangeException(nameof(at));
        start += _start;
        return _from.Aggregate(start..(start + len));
    }

    public void Clear() => _from.Clear();

    public void CopyTo(T[] array, int arrayIndex)
    {
        if (arrayIndex < 0 || arrayIndex >= array.Length)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        if (array.Length - arrayIndex < _length)
            throw new ArgumentException("Destination array is too short", nameof(array));
        for (var i = 0; i < _length; i++)
            array[i + arrayIndex] = _from[i + _start];
    }

    public void CopyTo(Array array, int index)
    {
        if (array is not T[] arr)
            throw new ArgumentException($"Wrong type of array {array.GetType().Name}, expected {typeof(T[]).Name}");
        CopyTo(arr, index);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<T> GetEnumerator() => _from.Skip(_start).Take(_length).GetEnumerator();

    #region Searching
    public bool Contains(object? value) => value is T t && Contains(t);
    public bool Contains(T item) => _length != 0 && IndexOf(item) > -1;
    public int IndexOf(object? value) => value is T t ? IndexOf(t) : -1;
    public int IndexOf(T item) => IndexOf(item, 0);
    public int IndexOf(T item, int startIndex)
    {
        if (startIndex < 0 || startIndex >= _length) throw new ArgumentOutOfRangeException(nameof(startIndex));
        var index = _from.IndexOf(item, _start + startIndex);
        if (index == -1 || index < _start || index >= _start + _length) return -1;
        return index - _start;
    }
    #endregion

    #region Not supported
    public void Add(T item) => throw new NotSupportedException();
    public int Add(object? value) => throw new NotSupportedException();
    public void Insert(int index, T item) => throw new NotSupportedException();
    public void Insert(int index, object? value) => throw new NotSupportedException();
    public bool Remove(T item) => throw new NotSupportedException();
    public void Remove(object? value) => throw new NotSupportedException();
    public void RemoveAt(int index) => throw new NotSupportedException();
    #endregion
}
