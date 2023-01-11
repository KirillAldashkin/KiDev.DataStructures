namespace KiDev.DataStructures;

/// <summary>
/// Represents a collection of objects that can be partially aggregated.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public interface IAggregableList<T> : IList<T>
{
    /// <summary>
    /// Gets or sets the element at the specified index.
    /// </summary>
    /// <param name="index">Index of the element to get or set.</param>
    /// <returns>The element at the specified index.</returns>
    T this[Index index] { get; set; }

    /// <summary>
    /// Aggregates all elements in the specified range.
    /// </summary>
    /// <param name="at">The range in which the elements should be aggregated.</param>
    /// <returns>Result of aggregation of all elements in the specified range.</returns>
    T Aggregate(Range at);

    /// <summary>
    /// Determines the index of a specific item in the <see cref="IAggregableList{T}"/>.
    /// </summary>
    /// <param name="item">The object to locate in the in <see cref="IAggregableList{T}"/></param>
    /// <param name="startIndex">The index from which the search will begin.</param>
    /// <returns>The index of <c>item</c> if found in the list; otherwise, -1.</returns>
    int IndexOf(T item, int startIndex);
}
