namespace KiDev.DataStructures;

/// <summary>
/// Provides a set of <c>static</c> methods for working with 
/// <see cref="IAggregableList{T}"/>, <see cref="IReadOnlyAggregableList{T}"/>
/// </summary>
public static class SegmentTreeExtensions
{
    /// <summary>
    /// Creates a subsequence over the current <see cref="IAggregableList{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="from">An <see cref="IAggregableList{T}"/> instance to create a subsequence over.</param>
    /// <param name="range">Range that the returned subsequence will cover.</param>
    /// <returns>An instance of <see cref="IAggregableList{T}"/> representing the <paramref name="from"/> subsequence.</returns>
    /// <remarks>This is a view, not a copy. All changes made to the source list will be presented in the returned list <b>(and vice versa).</b></remarks>
    public static IAggregableList<T> SubList<T>(this IAggregableList<T> from, Range range)
    {
        var (start, length) = range.GetOffsetAndLength(from.Count);
        return from.SubList(start, length);
    }

    /// <summary>
    /// Creates a subsequence over the current <see cref="IAggregableList{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="from">An <see cref="IAggregableList{T}"/> instance to create a subsequence over.</param>
    /// <param name="start">The index of the first element in <paramref name="from"/> that the returned subsequence will cover.</param>
    /// <param name="length">Length of a returned subsequence.</param>
    /// <returns>An instance of <see cref="IAggregableList{T}"/> representing the <paramref name="from"/> subsequence.</returns>
    /// <remarks>This is a view, not a copy. All changes made to the source list will be presented in the returned list <b>(and vice versa).</b></remarks>
    public static IAggregableList<T> SubList<T>(this IAggregableList<T> from, int start, int length) =>
        new AggregableSubList<T>(from, start, length);

    /// <summary>
    /// Creates a read-only subsequence over the current <see cref="IAggregableList{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="from">An <see cref="IAggregableList{T}"/> instance to create a subsequence over.</param>
    /// <param name="range">Range that the returned subsequence will cover.</param>
    /// <returns>An instance of <see cref="IReadOnlyAggregableList{T}"/> representing the <paramref name="from"/> subsequence.</returns>
    /// <remarks>This is a view, not a copy. All changes made to the source list will be presented in the returned list.</remarks>
    public static IReadOnlyAggregableList<T> ReadOnlySubList<T>(this IAggregableList<T> from, Range range)
    {
        var (start, length) = range.GetOffsetAndLength(from.Count);
        return from.ReadOnlySubList(start, length);
    }

    /// <summary>
    /// Creates a read-only subsequence over the current <see cref="IAggregableList{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="from">An <see cref="IAggregableList{T}"/> instance to create a subsequence over.</param>
    /// <param name="start">The index of the first element in <paramref name="from"/> that the returned subsequence will cover.</param>
    /// <param name="length">Length of a returned subsequence.</param>
    /// <returns>An instance of <see cref="IReadOnlyAggregableList{T}"/> representing the <paramref name="from"/> subsequence.</returns>
    /// <remarks>This is a view, not a copy. All changes made to the source list will be presented in the returned list.</remarks>
    public static IReadOnlyAggregableList<T> ReadOnlySubList<T>(this IAggregableList<T> from, int start, int length) =>
        new AggregableSubList<T>(from, start, length);
}
