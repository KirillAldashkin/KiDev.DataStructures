namespace KiDev.DataStructures.Benchmarks;

/// <summary>
/// Naive array-based implementation of <see cref="IDisjointSetUnion{TValue, THandle}"/>
/// </summary>
public class ArrayDSU : IDisjointSetUnion<int, int>
{
    private int[] groups;
    public ArrayDSU(int length)
    {
        groups = new int[length];
        for(var i = 0; i < length; i++) 
            groups[i] = i;
    }

    public bool InSameSet(int a, int b) => groups[a] == groups[b];

    public int SetOf(int a) => groups[a];

    public bool Unite(int a, int b)
    {
        var (setA, setB) = (groups[a], groups[b]);
        if (setA == setB) return false;
        for (int i = 0; i < groups.Length; i++)
            if (groups[i] == setA) groups[i] = setB;
        return true;
    }

    public IReadOnlyDictionary<int, IEnumerable<int>> Group(IEnumerable<int> elements) =>
        throw new NotImplementedException();
}