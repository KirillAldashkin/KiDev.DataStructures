[![Nuget](https://img.shields.io/nuget/v/kidev.datastructures?style=plastic)](https://www.nuget.org/packages/KiDev.DataStructures)

# KiDev.DataStructures
#### A lot of specific, but very fast data structures.

## Segment tree
`Array`-like container that is slower when setting, but extremely fast when aggregating.
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

## Disjoint set union
Ñontainer that can check whether two items belong to the same set, and also fast at combining items into sets.
```cs
var dsu = new DisjointSetUnionDictionary<string>();
dsu.Unite("foo", "bar");
dsu.Unite("bob", "try");
Console.WriteLine(dsu.InSameSet("foo", "bar")); // true
Console.WriteLine(dsu.InSameSet("bar", "bob")); // false
dsu.Unite("bar", "bob");
Console.WriteLine(dsu.InSameSet("foo", "try")); // true
```
~900 times faster than the regular array implementation when randomly combining 50K `int`'s into sets:

```
|           Method |            Mean |        Error |       StdDev |    Ratio |
|----------------- |----------------:|-------------:|-------------:|---------:|
|   SequalUniteDSU |     2,448.58 us |    11.798 us |     9.852 us |     1.00 |
| SequalUniteArray | 2,331,318.12 us | 8,144.394 us | 7,618.271 us |   951.69 |
|   RandomUniteDSU |     6,525.84 us |    22.288 us |    17.401 us |     2.67 |
| RandomUniteArray | 2,603,163.78 us | 9,299.580 us | 7,765.571 us | 1,063.14 |
```