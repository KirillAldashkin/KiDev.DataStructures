namespace KiDev.DataStructures.Benchmarks;

/// <summary>
/// Array-based implementation of <see cref="IDisjointSetUnion{TValue, THandle}"/>
/// Only used in benchmarks
/// </summary>
public class ArrayDSU : IDisjointSetUnion<int, int>
{
    // For an integer 'i', 'groups[i]' is the index of the set in which 'i' is located 
    private int[] groups;
    public ArrayDSU(int length)
    {
        groups = new int[length];
        // By default, each element is in its own set.
        for (var i = 0; i < length; i++) 
            groups[i] = i;
    }

    public bool InSameSet(int a, int b) => groups[a] == groups[b];

    public int SetOf(int a) => groups[a];

    public bool Unite(int a, int b)
    {
        // Check whether A and B are in the same set already
        var (setA, setB) = (groups[a], groups[b]);
        if (setA == setB) return false;
        // Move all the items of set A to set B
        for (int i = 0; i < groups.Length; i++)
            if (groups[i] == setA) groups[i] = setB;
        return true;
    }

    public IReadOnlyDictionary<int, IEnumerable<int>> Group(IEnumerable<int> elements) =>
        throw new NotImplementedException();
}