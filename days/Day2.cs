namespace Aoc._2024.days;

public class Day2 : IAocDay<int>
{
    public int DayNumber { get; } = 2;
    public int ExpectedTestResultPart1 { get; } = 2;
    public int? ExpectedTestResultPart2 { get; } = 4;
    public int SolvePart1(string path) =>
        File.ReadLines(path)
            .Select(line => line.Split(" ").Select(int.Parse).ToList())
            .Count(IsSafe);

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

    private static bool IsSafe(List<int> levels) =>
        !Enumerable.Range(1, levels.Count - 1)
            .Select(idx => (Previous: levels[idx - 1], Current: levels[idx]))
            .Any(item => BreaksRule(levels, item));

    private static bool BreaksRule(List<int> levels, (int Previous, int Current) item)
    {
        var shouldBeAscending = levels[0] < levels[1];
        var shouldBeDescending = levels[0] > levels[1];
        
        var isAscending = item.Current > item.Previous;
        var isDescending = item.Current < item.Previous;
        
        var difference = Math.Abs(item.Previous - item.Current);
        
        return shouldBeAscending && !isAscending ||
               shouldBeDescending && !isDescending ||
               difference is < 1 or > 3;
    }
}