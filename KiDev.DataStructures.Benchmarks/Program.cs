using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Reflection;

var classes = Assembly.GetExecutingAssembly().GetTypes().Where(IsBenchmarkType).ToArray();
Console.WriteLine("Select benchmark:");
for(var i = 0; i < classes.Length; i++)
    Console.WriteLine($"{i+1}) {classes[i].Name}");
Console.Write("Write index: ");
if(!int.TryParse(Console.ReadLine(), out var index) || index < 1 || index > classes.Length)
{
    Console.WriteLine($"Wrong index");
    return;
}
BenchmarkRunner.Run(classes[index - 1]);

static bool IsBenchmarkType(Type type) => type.GetCustomAttributes<MemoryDiagnoserAttribute>().Any();