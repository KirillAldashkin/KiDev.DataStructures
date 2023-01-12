using BenchmarkDotNet.Attributes;

namespace KiDev.DataStructures.Benchmarks;

[MemoryDiagnoser(true)]
public class Instantiation
{
    [Params(1_000, 10_000, 100_000)]
    public static int Size { get; set; }

    [Benchmark(Baseline = true)]
    public int[] NewArray() => new int[Size];

    [Benchmark]
    public SegmentTree<int> NewSegmentTree() => new(Size, Math.Max);

    [Benchmark]
    public DisjointSetUnionDictionary<int> NewDisjointSetUnionDictionary() => new(Size);
}
