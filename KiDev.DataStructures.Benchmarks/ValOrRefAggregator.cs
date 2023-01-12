using BenchmarkDotNet.Attributes;

namespace KiDev.DataStructures.Benchmarks;

[MemoryDiagnoser(true)]
public class ValOrRefAggregator
{
    // Total amount of elements
    [Params(200, 20_000, 2_000_000)]
    public static int Size { get; set; }

#pragma warning disable CS8618
    private static SegmentTree<HexaLong> byVal, byRef;
#pragma warning restore CS8618

    [GlobalSetup]
    public static void Initialize()
    {
        var rnd = new Random(683475);
        var arr = new HexaLong[Size];
        for (var i = 0; i < Size; i++)
            arr[i] = new(rnd.NextInt64(), rnd.NextInt64(), rnd.NextInt64(), rnd.NextInt64(),
                         rnd.NextInt64(), rnd.NextInt64(), rnd.NextInt64(), rnd.NextInt64(),
                         rnd.NextInt64(), rnd.NextInt64(), rnd.NextInt64(), rnd.NextInt64(),
                         rnd.NextInt64(), rnd.NextInt64(), rnd.NextInt64(), rnd.NextInt64());

        byVal = new(Size, HexaLong.Add, arr);
        byRef = new(Size, HexaLong.AddRef, arr);
    }

    [Benchmark]
    public HexaLong SumByVal() => byVal.Aggregate(..);

    [Benchmark(Baseline = true)]
    public HexaLong SumByRef() => byRef.Aggregate(..);


    public struct HexaLong
    {
        private long _v0, _v1, _v2, _v3, _v4, _v5, _v6, _v7;
        private long _v8, _v9, _v10, _v11, _v12, _v13, _v14, _v15;

        public HexaLong(long v0, long v1, long v2, long v3, long v4, long v5, long v6, long v7, 
                        long v8, long v9, long v10, long v11, long v12, long v13, long v14, long v15)
        {
            _v0 = v0; _v1 = v1; _v2 = v2;   _v3 = v3;   _v4 = v4;   _v5 = v5;   _v6 = v6;   _v7 = v7; 
            _v8 = v8; _v9 = v9; _v10 = v10; _v11 = v11; _v12 = v12; _v13 = v13; _v14 = v14; _v15 = v15;
        }

        public static HexaLong Add(HexaLong a, HexaLong b) => new(
            unchecked(a._v0 + b._v0),   unchecked(a._v1 + b._v1),
            unchecked(a._v2 + b._v2),   unchecked(a._v3 + b._v3),
            unchecked(a._v4 + b._v4),   unchecked(a._v5 + b._v5),
            unchecked(a._v6 + b._v6),   unchecked(a._v7 + b._v7),
            unchecked(a._v8 + b._v8),   unchecked(a._v9 + b._v9),
            unchecked(a._v10 + b._v10), unchecked(a._v11 + b._v11),
            unchecked(a._v12 + b._v12), unchecked(a._v13 + b._v13),
            unchecked(a._v14 + b._v14), unchecked(a._v15 + b._v15)
        );

        public static void AddRef(in HexaLong a, in HexaLong b, out HexaLong sum)
        {
            sum._v0 = unchecked(a._v0 + b._v0);    sum._v1 = unchecked(a._v1 + b._v1);
            sum._v2 = unchecked(a._v2 + b._v2);    sum._v3 = unchecked(a._v3 + b._v3);
            sum._v4 = unchecked(a._v4 + b._v4);    sum._v5 = unchecked(a._v5 + b._v5);
            sum._v6 = unchecked(a._v6 + b._v6);    sum._v7 = unchecked(a._v7 + b._v7);
            sum._v8 = unchecked(a._v8 + b._v8);    sum._v9 = unchecked(a._v9 + b._v9);
            sum._v10 = unchecked(a._v10 + b._v10); sum._v11 = unchecked(a._v11 + b._v11);
            sum._v12 = unchecked(a._v12 + b._v12); sum._v13 = unchecked(a._v13 + b._v13);
            sum._v14 = unchecked(a._v14 + b._v14); sum._v15 = unchecked(a._v15 + b._v15);
        }
    }
}
