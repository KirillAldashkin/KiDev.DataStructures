namespace KiDev.DataStructures;

/// <summary>
/// Represents a read-only container of items that are combined into sets.
/// </summary>
/// <typeparam name="TValue">The type of items in the sets.</typeparam>
/// <typeparam name="THandle">The type of the set handle.</typeparam>
/// <remarks>
/// When implementing this interface, you should also implement 
/// <see cref="object.Equals(object)"/> method in <typeparamref name="THandle"/> class.
/// </remarks>
public interface IReadOnlyDisjointSetUnion<TValue, THandle>
{
    /// <summary>
    /// Determines whether two <typeparamref name="TValue"/> instances are in the same set.
    /// </summary>
    /// <param name="a">First instance of <typeparamref name="TValue"/>.</param>
    /// <param name="b">Second instance of <typeparamref name="TValue"/>.</param>
    /// <returns>Whether <paramref name="a"/> and <paramref name="b"/> are in the same set.</returns>
    bool InSameSet(TValue a, TValue b);

    /// <summary>
    /// Returns the set handle in which given item is located.
    /// </summary>
    /// <param name="a">The item whose set needs to be defined.</param>
    /// <returns>The handle of the set that contains this item.</returns>
    /// <remarks>
    /// Handles of different items can be compared using the 
    /// <see cref="object.Equals(object)"/> method, which will return 
    /// <c>true</c> if the handles belong to the same set.
    /// </remarks>
    THandle SetOf(TValue a);

    /// <summary>
    /// Distributes item in a given <see cref="IEnumerable{TValue}"/> into sets.
    /// </summary>
    /// <param name="elements">Items that need to be distributed into the sets.</param>
    /// <returns>Dictionary with all the sets over which these items are distributed.</returns>
    IReadOnlyDictionary<THandle, IEnumerable<TValue>> Group(IEnumerable<TValue> elements);
}
