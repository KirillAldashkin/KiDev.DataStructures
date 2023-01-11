using BenchmarkDotNet.Attributes;
using KiDev.DataStructures;

[MemoryDiagnoser(false)]
public class GetSet
{
    // Total amount of elements
    [Params(10, 1_000, 100_000, 10_000_000)]
    public static int Size { get; set; }

#pragma warning disable CS8618
    private static Random rndIndex, rndValue;
    private static List<int> list; // Source list
    private static SegmentTree<int> tree; // Source segment tree
#pragma warning restore CS8618

    [GlobalSetup]
    public static void Initialize()
    {
        var rnd = new Random(12345);
        tree = new(Size, Math.Max);
        list = new(Size);
        for (int i = 0; i < Size; i++)
        {
            var val = rnd.Next();
            list.Add(val);
            tree[i] = val;
        }
        rndIndex = new(34567);
        rndValue = new(76543);
    }

    [Benchmark(Baseline = true)]
    public int RandomGetList() => list[rndIndex.Next(Size)];

    [Benchmark]
    public int RandomSetList() => list[rndIndex.Next(Size)] = rndValue.Next();

    [Benchmark]
    public int RandomGetTree() => tree[rndIndex.Next(Size)];

    [Benchmark]
    public int RandomSetTree() => tree[rndIndex.Next(Size)] = rndValue.Next();
}