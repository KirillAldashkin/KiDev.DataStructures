[![Nuget](https://img.shields.io/nuget/v/kidev.datastructures?style=plastic)](https://www.nuget.org/packages/KiDev.DataStructures)

# KiDev.DataStructures
#### A lot of specific, but very fast data structures.

## SegmentTree\<T\>
`List<T>`-like container that is slower when setting, but extremely fast when aggregating.
```cs
using KiDev.DataStructures;

var rnd = new Random();
var array = new int[50];
var tree = new SegmentTree<int>(50, (a, b) => a + b); // sum function as aggregator
for(var i = 0; i < array.Length; i++)
    tree[i] = array[i] = rnd.Next(-10000, 10000);
Console.WriteLine($"{tree.Aggregate(10..30)} == {array.Take(10..30).Sum()}");
```
~90000 times faster than the `for` loop over an array when aggregating 10M `int`'s:
```
|        Method |              Mean |          Error |         StdDev |        Ratio |
|-------------- |------------------:|---------------:|---------------:|-------------:|
|      ArrayFor |   7,985,157.48 ns |  88,720.653 ns |  78,648.548 ns |    92,003.40 |
|     ArrayLinq | 129,321,418.33 ns | 929,449.946 ns | 869,408.056 ns | 1,490,157.57 |
| TreeAggregate |          86.77 ns |       0.408 ns |       0.340 ns |         1.00 |
```