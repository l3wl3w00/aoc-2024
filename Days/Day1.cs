using Aoc._2024.Common;

namespace Aoc._2024.Days;

public class Day1 : IAocDay<int>
{
    public int DayNumber { get; } = 1;
    public int ExpectedTestResultPart1 { get; } = 11;
    public int? ExpectedTestResultPart2 { get; } = 31;
    public int SolvePart1(string path)
    {
        List<int> leftLocations = [];
        List<int> rightLocations = [];
        File.ReadLines(path)
            .Select(l => l.Split("   ").Select(int.Parse).ToArray())
            .ForEach(pair =>
            {
                leftLocations.Add(pair[0]);
                rightLocations.Add(pair[1]);
            });

        leftLocations.Sort();
        rightLocations.Sort();

        return Enumerable
            .Range(0, leftLocations.Count)
            .Sum(i => Math.Abs(leftLocations[i] - rightLocations[i]));
    }

    public int SolvePart2(string path)
    {
        List<int> leftLocations = [];
        List<int> rightLocations = [];
        File.ReadLines(path)
            .Select(l => l.Split("   ").Select(int.Parse).ToArray())
            .ForEach(pair =>
            {
                leftLocations.Add(pair[0]);
                rightLocations.Add(pair[1]);
            });
        
        return leftLocations.Sum(ll => ll * rightLocations.Count(rl => rl == ll));
    }
}