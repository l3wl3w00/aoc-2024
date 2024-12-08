using System.Collections.Frozen;

namespace Aoc._2024.Days;

using Vec2 = Vector2<int>;
public class Day8 : IAocDay<int>
{
    public int DayNumber { get; } = 8;
    public int ExpectedTestResultPart1 { get; } = 14;
    public int? ExpectedTestResultPart2 { get; } = 34;

    public int SolvePart1(string path) => Solve(path, true);
    public int SolvePart2(string path) => Solve(path, false);

    private int Solve(string path, bool checkTwiceDistance)
    {
        var world = File.ReadLines(path)
            .SelectMany((line, row) => line.Select((c, col) => (Pos: new Vec2(col, row), Frequency: c)))
            .ToFrozenDictionary(c => c.Pos, c => c.Frequency);

        var worldByFrequencies = world
            .GroupBy(p => p.Value)
            .Where(g => g.Key != '.')
            .ToFrozenDictionary(
                p => p.Key,
                p => world
                    .Where(f => f.Value == p.Key)
                    .Select(f => f.Key)
                    .ToList());
        return world.Keys.Count(pos => worldByFrequencies.Values.Any(f => IsAntinode(pos, f, checkTwiceDistance)));
    }
    
    private bool IsAntinode(Vec2 pos, List<Vec2> frequencyPositions, bool checkTwiceDistance) =>
        frequencyPositions
            .Any(freqPos => frequencyPositions
                .Where(freqPos2 => freqPos2 != freqPos)
                .Any(freqPos2 =>
                {
                    var pos1Diff = freqPos - pos;
                    var pos2Diff = freqPos2 - pos;
                    var enclosedArea = pos1Diff.X * pos2Diff.Y - pos1Diff.Y * pos2Diff.X;
                    var arePointsOnTheSameLine = Math.Abs(enclosedArea) < 1e-7f;

                    var isTwiceDistance =
                        Math.Abs(pos.DistanceTo(freqPos) - 2 * pos.DistanceTo(freqPos2)) < 0.0001 ||
                        Math.Abs(2 * pos.DistanceTo(freqPos) - pos.DistanceTo(freqPos2)) < 0.0001;

                    return arePointsOnTheSameLine && (!checkTwiceDistance || isTwiceDistance);
                }));
}