namespace KiDev.DataStructures.Tests;

[TestClass]
public class DisjointSetUnionIndexOnlyTests
{
    [TestMethod("Initialization, getting and setting (value type)")]
    public void InitGetSetValueType()
    {
        const int SubTests = 200;
        const int Min = 20;
        const int Max = 50;

        var rnd = new Random(9876);
        for (var testI = 0; testI < SubTests; testI++)
        {
            var dsu = new DisjointSetUnionIndexOnly(Max);
            for (var i = 0; i < SubTests; i++)
            {
                int a = rnd.Next(Max), b = rnd.Next(Max);
                Assert.AreEqual(a == b, dsu.InSameSet(a, b));
                Assert.AreEqual(a == b, dsu.SetOf(a) == dsu.SetOf(b));
            }

            var size1 = rnd.Next(0, Max - 1);
            var size2 = Max - 1 - size1;
            var array = TestHelpers.GenerateUniqueArray(rnd, Max - 1, 0, Max);
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
