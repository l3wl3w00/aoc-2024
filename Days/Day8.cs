using System.Collections.Frozen;

namespace Aoc._2024.Days;

using Vec2 = Vector2<int>;
public class Day8 : IAocDay<int>
{
    public int DayNumber { get; } = 8;
    public int ExpectedTestResultPart1 { get; } = 14;
    public int? ExpectedTestResultPart2 { get; } = 34;

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
    public int SolvePart1(string path) => Solve(path, true);
    public int SolvePart2(string path) => Solve(path, false);

    private bool IsAntinode(Vec2 pos, List<Vec2> frequencyPositions, bool checkTwiceDistance) =>
        frequencyPositions
            .Join(frequencyPositions, x => x, x => x, (a, b) => (Pos1: a, Pos2: b))
            .Where(frequencyPair => frequencyPair.Pos1 != frequencyPair.Pos2)
            .Where(frequencyPair =>
            {
                var distanceToPos1 = pos.DistanceTo(frequencyPair.Pos1);
                var distanceToPos2 = pos.DistanceTo(frequencyPair.Pos2);
                var isTwiceDistance = 
                    Math.Abs(distanceToPos1 - 2 * distanceToPos2) < 0.0001 ||
                    Math.Abs(2 * distanceToPos1 - distanceToPos2) < 0.0001;
                return !checkTwiceDistance || isTwiceDistance;
            })
            .Where(frequencyPair =>
            {
                var pos1Diff = frequencyPair.Pos1 - pos;
                var pos2Diff = frequencyPair.Pos2 - pos;
                var enclosedArea = pos1Diff.X * pos2Diff.Y - pos1Diff.Y * pos2Diff.X;
                var arePointsOnTheSameLine = Math.Abs(enclosedArea) < 1e-7f;
                return arePointsOnTheSameLine;
            })
            .Any();
}