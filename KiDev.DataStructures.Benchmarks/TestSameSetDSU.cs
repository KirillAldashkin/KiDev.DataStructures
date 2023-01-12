using BenchmarkDotNet.Attributes;

namespace KiDev.DataStructures.Benchmarks;

[MemoryDiagnoser(true)]
public class TestSameSetDSU
{
    [Params(10, 1_000, 100_000)]
    public static int Size { get; set; }

#pragma warning disable CS8618
    private static (int a, int b)[] randomQueries;
    private static (int a, int b)[] sequalQueries;
    private static ArrayDSU array;
    private static DisjointSetUnionDictionary<int> dsu;
    private static bool[] answers;
#pragma warning restore CS8618

    [GlobalSetup]
    public static void Setup()
    {
        var rnd = new Random(124816);
        randomQueries = Enumerable.Range(0, Size - 1).Select(_ => (rnd.Next(Size), rnd.Next(Size))).ToArray();
        sequalQueries = Enumerable.Range(0, Size - 1).Select(i => (i, i + 1)).ToArray();

        array = new(Size);
        dsu = new(Size);
        answers = new bool[Size - 1];
        var rndUnite = Enumerable.Range(0, Size * 10).Select(_ => (rnd.Next(Size), rnd.Next(Size)));
        foreach(var (a, b) in rndUnite)
        {
            array.Unite(a, b);
            dsu.Unite(a, b);
        }
    }

    [Benchmark(Baseline = true)]
    public void RandomTestDSU()
    {
        for (var i = 0; i < Size - 1; i++)
        {
            var (a, b) = randomQueries[i];
            answers[i] = dsu.InSameSet(a, b);
        }
    }

    [Benchmark]
    public void RandomTestNaive()
    {
        for (var i = 0; i < Size - 1; i++)
        {
            var (a, b) = randomQueries[i];
            answers[i] = array.InSameSet(a, b);
        }
    }

    [Benchmark]
    public void SequalTestDSU()
    {
        for (var i = 0; i < Size - 1; i++)
        {
            var (a, b) = sequalQueries[i];
            answers[i] = dsu.InSameSet(a, b);
        }
    }

    [Benchmark]
    public void SequalTestNaive()
    {
        for (var i = 0; i < Size - 1; i++)
        {
            var (a, b) = sequalQueries[i];
            answers[i] = array.InSameSet(a, b);
        }
    }
}
