namespace KiDev.DataStructures.Tests;

internal static class TestHelpers
{

    public static void AllPairs<T>(T[] array, Action<T, T> func)
    {
        for (var i = 0; i < array.Length; i++)
            for (var j = 0; j < i; j++)
                func(array[i], array[j]);
    }

    public static string GenerateString(Random rnd, char[] prefix, int strLen)
    {
        return string.Create(strLen, 1, (str, _) =>
        {
            prefix.CopyTo(str);
            for (var i = prefix.Length; i < str.Length; i++)
                str[i] = (char)rnd.Next(32, 128);
        });
    }

    public static string[] GenerateStringArray(Random rnd, int arrLen, int strLen)
    {
        if (strLen < 6) throw new ArgumentOutOfRangeException(nameof(strLen));
        var prefix = new char[6];
        Array.Fill(prefix, ' ');
        var ret = new string[arrLen];
        for (var i = 0; i < arrLen; i++)
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

    public static int[] GenerateUniqueArray(Random rnd, int len, int min, int max)
    {
        var ret = new int[len];
        var values = new List<int>(max-min);
        for (int i = min; i < max; i++) values.Add(i);

        for (var i = 0; i < len; i++)
        {
            var j = rnd.Next(0, values.Count);
            ret[i] = values[j];
            values.RemoveAt(j);
        }
        return ret;
    }

    public static bool EnumerablesEqual<T>(IEnumerable<T> a, IEnumerable<T> b) =>
        a.Zip(b).All(((T, T) c) => c.Item1!.Equals(c.Item2));

    public static int[][] MakeArrays(Random rnd, int count, int minLen, int maxLen, int? min = null, int? max = null)
    {
        min ??= int.MinValue;
        max ??= int.MaxValue;
        var ret = new int[count][];
        for (var i = 0; i < count; i++)
            ret[i] = Enumerable.Range(0, rnd.Next(minLen, maxLen))
                               .Select(t => rnd.Next(min.Value, max.Value))
                               .ToArray();
        return ret;
    }
}