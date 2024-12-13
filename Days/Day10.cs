using System.Collections.Frozen;
using System.Collections.Immutable;
using Aoc._2024.Common;

namespace Aoc._2024.Days;
using Vec2 = Vector2<int>;
public class Day10 : IGriddedAocDay<int, int, int>
{
    public int DayNumber { get; } = 10;
    public int ExpectedTestResultPart1 { get; } = 36;
    public int? ExpectedTestResultPart2 { get; } = 81;

    public int MapCharToField(char c) => c - '0';

    public int SolvePart1(Grid<int, int> grid)
    {
        return grid
            .Where(f => f.Value == 0)
            .Sum(f => GetReachable9s(f.Key, f.Value, grid).ToHashSet().Count);
    }

    public int SolvePart2(Grid<int, int> grid)
    {
        return grid
            .Where(f => f.Value == 0)
            .Sum(f => GetDistinctTrails(f.Key, f.Value, grid, [f.Key]).ToHashSet().Count);
    }

    private IEnumerable<Vec2> GetReachable9s(Vec2 trailHeadPos, int currentHeight, Grid<int, int> map)
    {
        if (map.Get(trailHeadPos) == 9 && currentHeight == 9)
        {
            return [trailHeadPos];
        }
        return Vec2.UpDownLeftRightDirs
            .Where(d => map.Get(trailHeadPos + d) == currentHeight + 1)
            .SelectMany(d => GetReachable9s(trailHeadPos + d, currentHeight + 1, map));
    }

    private IEnumerable<ImmutableList<Vec2>> GetDistinctTrails(
        Vec2 currentPos,
        int currentHeight, 
        Grid<int, int> map, 
        ImmutableList<Vec2> visitedPositions)
    {
        if (map.Get(currentPos) == 9 && currentHeight == 9)
        {
            return [visitedPositions];
        }
        return Vec2.UpDownLeftRightDirs
            .Select(d => currentPos + d)
            .Where(nextPos => map.Get(nextPos) == currentHeight + 1)
            .SelectMany(nextPos => GetDistinctTrails(
                nextPos, 
                currentHeight + 1,
                map,
                visitedPositions.Add(currentPos)));
    }
}