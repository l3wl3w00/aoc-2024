using System.Reflection;

namespace Aoc._2024;

class Program
{
    static void Main(string[] args)
    {
        var daysToRun = Enumerable.Range(1, 4).ToHashSet();

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
