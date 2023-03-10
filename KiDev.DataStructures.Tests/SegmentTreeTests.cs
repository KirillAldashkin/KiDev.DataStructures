namespace KiDev.DataStructures.Tests;

[TestClass]
public class SegmentTreeTests
{
    [TestMethod("Initialization and indexation")]
    public void TestInitAndIndexing()
    {
        const int SubTestCount = 200;
        const int MinLength = 2;
        const int MaxLength = 50;

        var rnd = new Random(1234);

        foreach (var test in TestHelpers.MakeArrays(rnd, SubTestCount, MinLength, MaxLength))
        {
            var def = rnd.Next();
            var tree1 = new SegmentTree<int>(test.Length, Math.Max, basis: def);
            Assert.ThrowsException<IndexOutOfRangeException>(() => tree1[-1]);
            Assert.ThrowsException<IndexOutOfRangeException>(() => tree1[tree1.Count]);
            for (int i = 0; i < test.Length; i++) Assert.AreEqual(tree1[i], def);
            for (int i = 0; i < test.Length; i++)
            {
                tree1[i] = test[i];
                Assert.AreEqual(tree1[i], test[i]);
            }
            var tree2 = new SegmentTree<int>(test.Length, Math.Max, test, def);
            Assert.IsTrue(TestHelpers.EnumerablesEqual(test, tree2));
        }
    }

    [TestMethod("Aggregation sum and Modify method")]
    public void TestAggregateModify()
    {
        const int SubTestCount = 100;
        const int MinLength = 20;
        const int MaxLength = 50;
        const int Min = -100000;
        const int Max = 100000;

        var rnd = new Random(2345);

        foreach (var test in TestHelpers.MakeArrays(rnd, SubTestCount, MinLength, MaxLength, Min, Max))
        {
            var tree = new SegmentTree<int>(test.Length, (a, b) => a + b);
            for (int i = 0; i < test.Length; i++) tree[i] = test[i];
            Assert.AreEqual(test.Sum(), tree.Aggregate(..));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => tree.Aggregate(..(test.Length + 1)));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => tree.Aggregate((MinLength / 2)..(test.Length + 1)));

            TestRandomRanges(test, tree);

            for (var j = 0; j < SubTestCount; j++)
            {
                var start = rnd.Next(0, tree.Count);
                var end = rnd.Next(start + 1, tree.Count + 1);
                var newValue = rnd.Next(Min, Max);

                test.AsSpan(start..end).Fill(newValue);
                tree.Modify(start..end, span => span.Fill(newValue));

                TestRandomRanges(test, tree);
            }
        }

        void TestRandomRanges(int[] test, SegmentTree<int> tree)
        {
            for (var j = 0; j < SubTestCount; j++)
            {
                var start = rnd.Next(0, tree.Count);
                var end = rnd.Next(start + 1, tree.Count + 1);
                var expect = test.Take(start..end).Sum();
                Assert.AreEqual(expect, tree.Aggregate(start..end));
                Assert.AreEqual(expect, tree.SubList(start..end).Aggregate(..));
            }
        }
    }

    [TestMethod("Sublist accessing, clear, IEnumerable")]
    public void TestClearEnumeration()
    {
        const int SubTestCount = 200;
        const int MinLength = 20;
        const int MaxLength = 50;

        var rnd = new Random(3456);

        foreach (var test in TestHelpers.MakeArrays(rnd, SubTestCount, MinLength, MaxLength))
        {
            var basis = rnd.Next();
            var tree = new SegmentTree<int>(test.Length, Math.Min, basis: basis);
            for (int i = 0; i < test.Length; i++) tree[i] = test[i];

            Assert.IsTrue(TestHelpers.EnumerablesEqual(test, tree));

            for (var j = 0; j < SubTestCount; j++)
            {
                var start = rnd.Next(0, tree.Count);
                var end = rnd.Next(start + 1, tree.Count + 1);
                var subArray = test.Take(start..end);
                var subList = tree.SubList(start..end);
                Assert.IsTrue(TestHelpers.EnumerablesEqual(subArray, subList));
            }

            tree.Clear();
            for (int i = 0; i < test.Length; i++) Assert.AreEqual(tree[i], basis);
        }

    }

    [TestMethod("CopyTo method")]
    public void TestCopyTo()
    {
        const int SubTestCount = 200;
        const int MinLength = 20;
        const int MaxLength = 50;
        const int Min = -100000;
        const int Max = 100000;

        var rnd = new Random(4567);

        foreach (var test in TestHelpers.MakeArrays(rnd, SubTestCount, MinLength, MaxLength, Min, Max))
        {
            var tree = new SegmentTree<int>(test.Length, (a, b) => a + b);
            for (int i = 0; i < test.Length; i++) tree[i] = test[i];
            var copy = new int[test.Length];
            tree.CopyTo(copy, 0);
            Assert.IsTrue(TestHelpers.EnumerablesEqual(test, copy));

            for (var j = 0; j < SubTestCount; j++)
            {
                var start = rnd.Next(0, tree.Count);
             
                var subList = tree.SubList(start..);
                var subCopy = new int[test.Length - start];
                subList.CopyTo(subCopy, 0);
                Assert.IsTrue(TestHelpers.EnumerablesEqual(subList, subCopy));
            }
        }
    }
}