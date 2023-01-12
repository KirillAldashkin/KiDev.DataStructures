namespace KiDev.DataStructures;

/// <summary>
/// Represents a method that gets direct access to the internal data of some structure.
/// </summary>
/// <typeparam name="T">The type of elements in the structure.</typeparam>
/// <param name="data">Provides direct access to the data.</param>
public delegate void SpanAccessor<T>(Span<T> data);