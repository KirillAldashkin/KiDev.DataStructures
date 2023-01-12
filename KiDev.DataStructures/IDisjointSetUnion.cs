namespace KiDev.DataStructures;

/// <summary>
/// Represents a container of items that can combine them into sets.
/// </summary>
/// <inheritdoc/>
public interface IDisjointSetUnion<TValue, THandle> : IReadOnlyDisjointSetUnion<TValue, THandle>
{
    /// <summary>
    /// Unites two items into one set, if they have not already been.
    /// </summary>
    /// <param name="a">First instance of <typeparamref name="TValue"/>.</param>
    /// <param name="b">Second instance of <typeparamref name="TValue"/>.</param>
    /// <returns>
    /// <c>true</c> - if the items were united into one set during this operation.
    /// <c>false</c> - if the items were already in the same set.
    /// </returns>
    bool Unite(TValue a, TValue b);
}
