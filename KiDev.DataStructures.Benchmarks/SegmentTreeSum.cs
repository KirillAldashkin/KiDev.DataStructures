using BenchmarkDotNet.Attributes;
using KiDev.DataStructures;
using System.Numerics;
using System.Runtime.InteropServices;

[MemoryDiagnoser(true)]
public class SegmentTreeSum
{
    // Maximum sum of all elements
    private const int MaxSum = 1_000_000_000;

    // Total amount of elements
    [Params(10, 1_000, 100_000, 10_000_000)]
    public static int Size { get; set; }

    // What part of the entire array should be aggregated (one second, one tenth, etc)
    [Params(1, 2, 10)]
    public static int Fraction { get; set; }

#pragma warning disable CS8618
    public static int[] array; // Source array
    public static SegmentTree<int> valTree; // Source segment tree
    public static SegmentTree<int> refTree; // Source segment tree
    public static int length; // Length of each subsequence
    public static int[] indices; // Indexes of the beginning of each subsequence
    public static int[] answers; // The results are recorded here so that the calculation code is not deleted during optimization
#pragma warning restore CS8618

    private static int Summ(int a, int b) => a + b;
    private static void SummRef(in int a, in int b, out int c) => c = a + b;

    [GlobalSetup]
    public static void Initialize()
    {
        // Fill containers with data
        var rnd = new Random(2345);
        array = new int[Size];
        for(int i = 0; i < Size; i++) array[i] = rnd.Next(-MaxSum / Size, MaxSum / Size);
        valTree = new(Size, Summ, array);
        refTree = new(Size, SummRef, array);
    
        // Generate random subsequence indices here so every benchmark will do the same calculations
        length = Size / Fraction;
        indices = Enumerable.Range(0, Fraction).Select(i => rnd.Next(0, Size - length + 1)).ToArray();
        answers = new int[Fraction];
    }

    [Benchmark]
    public void ArrayFor()
    {
        for (var i = 0; i < Fraction; i++)
        {
            var summ = 0;
            var offset = indices[i];
            for (int j = offset; j < offset + length; j++) summ += array[j];
            answers[i] = summ;
        }
    }

    [Benchmark]
    public void ArrayLinq()
    {
        for (var i = 0; i < Fraction; i++)
        {
            answers[i] = array.Skip(indices[i]).Take(length).Sum();
        }
    }

    [Benchmark]
    public void TreeByValAggregate()
    {
        for (var i = 0; i < Fraction; i++)
        {
            var offset = indices[i];
            answers[i] = valTree.Aggregate(offset..(offset+length));
        }
    }

    [Benchmark(Baseline = true)]
    public void TreeByRefAggregate()
    {
        for (var i = 0; i < Fraction; i++)
        {
            var offset = indices[i];
            answers[i] = refTree.Aggregate(offset..(offset+length));
        }
    }

    [Benchmark]
    public void Vectorized()
    {
        for (var i = 0; i < Fraction; i++)
        {
            var offset = indices[i];
            var data = array.AsSpan(offset, length);

            var vecs = MemoryMarshal.Cast<int, Vector<int>>(data);
            var sum = Vector<int>.Zero;
            for (var j = 0; j < vecs.Length; j++)
                sum += vecs[i];

            answers[i] = 0;
            for (var j = 0; j < Vector<int>.Count; j++)
                answers[i] += sum[j];
        }
    }
}
