using System.Reflection;
using System.Runtime.CompilerServices;

namespace Aoc._2024;

class Program
{
    static void Main(string[] args)
    {
        List<int> daysToRun = [1];

        var days = Assembly
            .GetExecutingAssembly()?
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IAocDay)) && !t.IsAbstract)
            .Select(t => (IAocDay?) Activator.CreateInstance(t))
            .ToList();

        days?.Where(d => daysToRun.Contains(d?.DayNumber ?? -1))
            .ToList()
            .ForEach(d => d?.Solve());
    }
}
