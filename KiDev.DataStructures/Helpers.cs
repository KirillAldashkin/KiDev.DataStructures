namespace KiDev.DataStructures;

internal static class Helpers
{
    public static void CheckBounds(int value, int min, int max, string valName)
    {
        if (min <= value && value < max) return;
        throw new ArgumentOutOfRangeException(nameof(valName));
    }

    public static IReadOnlyDictionary<THandle, IEnumerable<TValue>> 
        DefaultGroup<THandle, TValue>(IReadOnlyDisjointSetUnion<TValue, THandle> dsu, IEnumerable<TValue> elements)
    {
        var dict = new Dictionary<THandle, List<TValue>>();
        foreach (var item in elements)
        {
            var set = dsu.SetOf(item);
            if (!dict.TryGetValue(set, out var list))
            {
                list = new();
                dict[set] = list;
            }
            list.Add(item);
        }
        return (IReadOnlyDictionary<THandle, IEnumerable<TValue>>)dict;
    }

}