namespace KiDev.DataStructures.Tests;

[TestClass]
public class DisjointSetUnionDictionaryTests
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
            var array = TestHelpers.GenerateUniqueArray(rnd, size1 + size2, -10_000, 10_000);
            var array1 = array[..size1];
            var array2 = array[size1..];
            var indices1 = TestHelpers.GenerateUniqueArray(rnd, size1/3, 0, size1);
            var indices2 = TestHelpers.GenerateUniqueArray(rnd, size2/3, 0, size2);
            TestHelpers.AllPairs(indices1, (i, j) => {
                var (a, b) = (array1[i], array1[j]);
                var was1 = dsu.InSameSet(a, b);
                var was2 = dsu.SetOf(a) == dsu.SetOf(b);
                var became = dsu.Unite(a, b);
                Assert.AreNotEqual(was1, became);
                Assert.AreEqual(was1, was2);
            });
            TestHelpers.AllPairs(indices2, (i, j) => {
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
                var a = TestHelpers.GenerateString(rnd, Array.Empty<char>(), StringLength);
                var b = TestHelpers.GenerateString(rnd, Array.Empty<char>(), StringLength);
                Assert.AreEqual(a == b, dsu.InSameSet(a, b));
                Assert.AreEqual(a == b, dsu.SetOf(a) == dsu.SetOf(b));
            }

            var size1 = rnd.Next(MinLength, MaxLength);
            var size2 = rnd.Next(MinLength, MaxLength);
            var array = TestHelpers.GenerateStringArray(rnd, size1 + size2, StringLength);
            var array1 = array[..size1];
            var array2 = array[size1..];
            var indices1 = TestHelpers.GenerateUniqueArray(rnd, size1 / 3, 0, size1);
            var indices2 = TestHelpers.GenerateUniqueArray(rnd, size2 / 3, 0, size2);
            TestHelpers.AllPairs(indices1, (i, j) => {
                var (a, b) = (array1[i], array1[j]);
                var was1 = dsu.InSameSet(a, b);
                var was2 = dsu.SetOf(a) == dsu.SetOf(b);
                var became = dsu.Unite(a, b);
                Assert.AreNotEqual(was1, became);
                Assert.AreEqual(was1, was2);
            });
            TestHelpers.AllPairs(indices2, (i, j) => {
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
}
