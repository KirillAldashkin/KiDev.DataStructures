namespace KiDev.DataStructures.Tests;

[TestClass]
public class DisjointSetUnionTests
{
    [TestMethod("Initialization, getting and setting (value type)")]
    public void InitGetSetValueType()
    {
        const int SubTests = 200;
        const int MinLength = 20;
        const int MaxLength = 50;

        var rnd = new Random(9876);
        for (var testI = 0; testI < SubTests; testI++)
        {
            var dsu = new DisjointSetUnionDictionary<int>();
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

    [TestMethod("Initialization, getting and setting (reference type)")]
    public void InitGetSetReferenceType()
    {
        const int SubTests = 50;
        const int MinLength = 20;
        const int MaxLength = 50;
        const int StringLength = 50;
        
        var rnd = new Random(9876);
        for (var testI = 0; testI < SubTests; testI++)
        {
            var dsu = new DisjointSetUnionDictionary<string>();
            for (var i = 0; i < SubTests; i++)
            {
                var a = GenerateString(rnd, Array.Empty<char>(), StringLength);
                var b = GenerateString(rnd, Array.Empty<char>(), StringLength);
                Assert.AreEqual(a == b, dsu.InSameSet(a, b));
                Assert.AreEqual(a == b, dsu.SetOf(a) == dsu.SetOf(b));
            }

            var size1 = rnd.Next(MinLength, MaxLength);
            var size2 = rnd.Next(MinLength, MaxLength);
            var array = GenerateStringArray(rnd, size1 + size2, StringLength);
            var array1 = array[..size1];
            var array2 = array[size1..];
            var indices1 = GenerateUniqueArray(rnd, size1 / 3, 0, size1);
            var indices2 = GenerateUniqueArray(rnd, size2 / 3, 0, size2);
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

    private static string[] GenerateStringArray(Random rnd, int arrLen, int strLen)
    {
        if (strLen < 6) throw new ArgumentOutOfRangeException(nameof(strLen));
        var prefix = new char[6]; 
        Array.Fill(prefix, ' ');
        var ret = new string[arrLen];
        for(var i = 0; i < arrLen; i++)
        {
            ret[i] = GenerateString(rnd, prefix, strLen);
            prefix[0]++;
            for (var j = 0; j < 6 && prefix[j] > 127; j++)
            {
                prefix[j] = ' ';
                if (j < 5) prefix[j + 1]++;
            }
        }
        return ret;
    }

    private static string GenerateString(Random rnd, char[] prefix, int strLen)
    {
        return string.Create(strLen, 1, (str, _) =>
        {
            prefix.CopyTo(str);
            for (var i = prefix.Length; i < str.Length; i++)
                str[i] = (char)rnd.Next(32, 128);
        });
    }
}
