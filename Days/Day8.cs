﻿using System.Collections.Frozen;
using Aoc._2024.Common;

namespace Aoc._2024.Days;

using Vec2 = Vector2<int>;
public class Day8 : IGriddedAocDay<int, int, char>
{
    public int DayNumber { get; } = 8;
    public int ExpectedTestResultPart1 { get; } = 14;
    public int? ExpectedTestResultPart2 { get; } = 34;

    public int SolvePart1(Grid<int, char> grid) => Solve(grid, true);
    public int SolvePart2(Grid<int, char> grid) => Solve(grid, false);

    public char MapCharToField(char c) => c;

    private static int Solve(Grid<int, char> world, bool checkTwiceDistance)
    {
        var worldByFrequencies = world
            .GroupBy(p => p.Value)
            .Where(g => g.Key != '.')
            .ToFrozenDictionary(
                p => p.Key,
                p => world
                    .Where(f => f.Value == p.Key)
                    .Select(f => f.Key)
                    .ToList());
        return world.Positions.Count(pos => worldByFrequencies.Values.Any(f => IsAntinode(pos, f, checkTwiceDistance)));
    }
    private static bool IsAntinode(Vec2 pos, List<Vec2> frequencyPositions, bool checkTwiceDistance) =>
        frequencyPositions
            .Join(frequencyPositions, _ => 0, _ => 0, (p1, p2) => (Pos1: p1, Pos2: p2))
            .Where(frequencyPair => frequencyPair.Pos1 != frequencyPair.Pos2)
            .Any(frequencyPair =>
            {
                var (freqPos1, freqPos2) = frequencyPair;
                var pos1Diff = freqPos1 - pos;
                var pos2Diff = freqPos2 - pos;
                var enclosedArea = pos1Diff.X * pos2Diff.Y - pos1Diff.Y * pos2Diff.X;
                var arePointsOnTheSameLine = Math.Abs(enclosedArea) < 1e-7f;

                var isTwiceDistance =
                    Math.Abs(pos.DistanceTo(freqPos1) - 2 * pos.DistanceTo(freqPos2)) < 0.0001 ||
                    Math.Abs(2 * pos.DistanceTo(freqPos1) - pos.DistanceTo(freqPos2)) < 0.0001;

                return arePointsOnTheSameLine && (!checkTwiceDistance || isTwiceDistance);
            });
}