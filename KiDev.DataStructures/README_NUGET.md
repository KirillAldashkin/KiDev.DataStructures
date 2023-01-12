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
~90000 times faster than the `for` loop over an array when aggregating 10M `int`'s.

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
~900 times faster than the regular array implementation when randomly combining 50K `int`'s into sets.
