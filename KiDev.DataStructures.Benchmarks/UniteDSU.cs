using BenchmarkDotNet.Attributes;

namespace KiDev.DataStructures.Benchmarks;

[MemoryDiagnoser(true)]
public class UniteDSU
{
    [Params(1_000, 50_000)]
    public static int Size { get; set; }

#pragma warning disable CS8618
    private static (int a, int b)[] randomUnite;
    private static (int a, int b)[] sequalUnite;
#pragma warning restore CS8618

    [GlobalSetup]
    public static void Setup()
    {
        var rnd = new Random(124816);
        randomUnite = Enumerable.Range(0, Size - 1).Select(_ => (rnd.Next(Size), rnd.Next(Size))).ToArray();
        sequalUnite = Enumerable.Range(0, Size - 1).Select(i => (i, i + 1)).ToArray();
    }

    [Benchmark(Baseline = true)]
    public void SequalUniteDSU()
    {
        var union = new DisjointSetUnionDictionary<int>(Size);
        for (var i = 0; i < Size - 1; i++)
        {
            var (a, b) = sequalUnite[i];
            union.Unite(a, b);
        }
    }

    [Benchmark]
    public void SequalUniteNaive()
    {
        var array = new ArrayDSU(Size);
        for (var i = 0; i < Size - 1; i++)
        {
            var (a, b) = sequalUnite[i];
            array.Unite(a, b);
        }
    }

    [Benchmark]
    public void RandomUniteDSU()
    {
        var union = new DisjointSetUnionDictionary<int>(Size);
        for(var i = 0; i < Size - 1; i++)
        {
            var (a, b) = randomUnite[i];
            union.Unite(a, b);
        }
    }

    [Benchmark]
    public void RandomUniteNaive()
    {
        var array = new ArrayDSU(Size);
        for (var i = 0; i < Size - 1; i++)
        {
            var (a, b) = randomUnite[i];
            array.Unite(a, b);
        }
    }
}
