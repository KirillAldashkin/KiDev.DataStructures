namespace KiDev.DataStructures.Tests;

[TestClass]
public class DisjointSetUnionTests
{
    [TestMethod("Initialization, getting and setting")]
    public void InitGetSet()
    {
        const int SubTests = 200;
        const int MinLength = 20;
        const int MaxLength = 50;

        var rnd = new Random(9876);
        for (var testI = 0; testI < SubTests; testI++)
        {
            var dsu = new DisjointSetUnion<int>();
            for (var i = 0; i < SubTests; i++)
            {
                int a = rnd.Next(), b = rnd.Next();
                Assert.AreEqual(a == b, dsu.InSameSet(a, b));
                Assert.AreEqual(a == b, dsu.SetOf(a) == dsu.SetOf(b));
            }

            var size1 = rnd.Next(MinLength, MaxLength);
            var size2 = rnd.Next(MinLength, MaxLength);
            var array = GenerateUniqueArray(rnd, size1 + size2, int.MinValue, int.MaxValue);
            var array1 = array[..size1];
            var array2 = array[size1..];
            var indices1 = GenerateUniqueArray(rnd, size1/3, 0, size1);
            var indices2 = GenerateUniqueArray(rnd, size2/3, 0, size2);
            AllPairs(indices1, (i, j) => {
                var (a, b) = (array1[i], array1[j]);
                var was1 = dsu.InSameSet(a, b);
                var was2 = dsu.SetOf(a) == dsu.SetOf(b);
                var became = dsu.Unite(a, b);
                Assert.AreNotEqual(was1, became);
                Assert.AreEqual(was1, was2);
            });
            AllPairs(indices2, (i, j) => {
                var (a, b) = (array2[i], array2[j]);
                var was1 = dsu.InSameSet(a, b);
                var was2 = dsu.SetOf(a) == dsu.SetOf(b);
                var became = dsu.Unite(a, b);
                Assert.AreNotEqual(was1, became);
                Assert.AreEqual(was1, was2);
            });
            foreach (var a in indices1)
                foreach (var b in indices2)
                    Assert.IsFalse(dsu.InSameSet(array1[a], array2[b]));
        }
    }

    private static void AllPairs<T>(T[] array, Action<T, T> func)
    {
        for (var i = 0; i < array.Length; i++)
            for (var j = 0; j < i; j++)
                func(array[i], array[j]);
    }

    private static int[] GenerateUniqueArray(Random rnd, int len, int min, int max)
    {
        var ret = new int[len];
        for(var i = 0; i < len; i++)
        {
            var wrong = true;
            while (wrong)
            {
                wrong = false;
                ret[i] = rnd.Next(min, max);
                for(var j = 0; j < i; j++)
                    if (ret[i] == ret[j])
                    {
                        wrong = true;
                        break;
                    }
            }
        }
        return ret;
    }
}
