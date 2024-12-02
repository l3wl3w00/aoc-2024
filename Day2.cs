namespace Aoc._2024;

public class Day2 : IAocDay<int>
{
    public int DayNumber { get; } = 2;
    public int ExpectedTestResultPart1 { get; } = 2;
    public int? ExpectedTestResultPart2 { get; } = 4;
    public int SolvePart1(string path) =>
        File.ReadLines(path)
            .Select(line => line.Split(" ").Select(int.Parse).ToList())
            .Count(IsSafe);

    private static bool IsSafe(List<int> levels)
    {
        var previous = levels[0];
        var current = levels[1];
        if (previous == current)
        {
            return false;
        }
        var isFirst2Increasing = previous < current;
        
        for (var level = 1; level < levels.Count; level++)
        {
            current = levels[level];
            if (isFirst2Increasing && current <= previous)
            {
                return false;
            }
            
            if (!isFirst2Increasing && current >= previous)
            {
                return false;
            }
            
            var difference = Math.Abs(previous - current);
            if (difference is < 1 or > 3)
            {
                return false;
            }

            previous = current;
        }
        
        return true;
    }

    public int SolvePart2(string path) =>
        File.ReadLines(path)
            .Select(line => line.Split(" ").Select(int.Parse).ToList())
            .Count(IsAlmostSafe);

    private static bool IsAlmostSafe(List<int> levels) =>
        levels
            .Select((_, idx) => levels[..idx]
                .Concat(levels[(idx + 1)..])
                .ToList())
            .Any(IsSafe);
}