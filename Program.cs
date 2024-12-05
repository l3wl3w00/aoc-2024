using System.Reflection;

namespace Aoc._2024;

static class Program
{
    static void Main(string[] _)
    {
        var daysToRun = Enumerable.Range(5, 5).ToHashSet();

        Assembly.GetExecutingAssembly()?.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IAocDay)) && !t.IsAbstract)
            .Select(t => (IAocDay?)Activator.CreateInstance(t))
            .Where(d => daysToRun.Contains(d?.DayNumber ?? -1))
            .ForEach(d =>
            {
                Console.WriteLine($"Day {d!.DayNumber}:");
                d.Solve();
                Console.WriteLine();
            });
    }
}
