using System.Collections.Frozen;

namespace Aoc._2024.days;

public class Day4 : IAocDay<int>
{
    public int DayNumber { get; } = 4;
    public int ExpectedTestResultPart1 { get; } = 18;
    public int? ExpectedTestResultPart2 { get; } = 9;
    
    private const string Xmas = "XMAS";
    private static readonly int[][] DirectionsArray =
    [
        [1, 0], [1, -1], [1, 1],
        [-1, 0], [-1, 1], [-1, -1],
        [0, 1], [0, -1],
    ];

    private record struct IntVector2(int X, int Y)
    {
        public static IntVector2 operator +(IntVector2 a, IntVector2 b) => new(a.X + b.X, a.Y + b.Y);
        public static IntVector2 operator *(int scalar, IntVector2 a) => new(a.X * scalar, a.Y * scalar);
    }
    public int SolvePart1(string path)
    {
        var directions = DirectionsArray.Select(d => new IntVector2(d[0], d[1])).ToHashSet();
        var grid = File.ReadLines(path)
            .SelectMany((line, row) => line
                .Select((c, col) => new
                {
                    Pos = new IntVector2(col, row),
                    Value = c
                }))
            .ToFrozenDictionary(i => i.Pos, i => i.Value);

        return grid.Keys.Sum(pos => 
            directions
            .Select(dir => Xmas.Select((_, letterIdx) => GetAtPosOrNull(grid, pos + letterIdx * dir)))
            .Count(chars => string.Join("", chars) == Xmas));
    }

    public int SolvePart2(string path)
    {
        var mas = Xmas[1..];
        var verticalDirections = DirectionsArray
            .Where(d => Math.Abs(d[0] - d[1]) is 2 or 0)
            .Select(d => new IntVector2(d[0], d[1]))
            .ToHashSet();

        var grid = File.ReadLines(path)
            .SelectMany((line, row) => line
                .Select((c, col) => new
                {
                    Pos = new IntVector2(col, row),
                    Value = c
                }))
            .ToFrozenDictionary(i => i.Pos, i => i.Value);
        var crosses = grid.Keys
            .SelectMany(pos => verticalDirections.Select(dir => mas.Select((_, letterIdx) => pos + letterIdx * dir)))                
            .Where(cross => string.Join("", cross.Select(p => GetAtPosOrNull(grid, p))) == mas)
            .Select(cross => cross.ToList())
            .ToHashSet();
        return grid.Keys
            .Where(pos => GetAtPosOrNull(grid, pos) == 'A')
            .Count(pos => crosses.Count(cross => cross.Contains(pos)) == 2);
    }

    private static char? GetAtPosOrNull(FrozenDictionary<IntVector2, char> grid, IntVector2 pos)
    {
        var success = grid.TryGetValue(pos, out var result);
        return success ? result : null;
    }
}