using System.Collections.Frozen;
using System.Collections.Immutable;
using Aoc._2024.Common;

namespace Aoc._2024.Days;
using Vec2 = Vector2<short>;

public class Day6 : IGriddedAocDay<int, short, char>
{
    public int DayNumber { get; } = 6;
    public int ExpectedTestResultPart1 { get; } = 41;
    public int? ExpectedTestResultPart2 { get; } = 6;

    private static readonly HashSet<char> GuardChars = ['^', '>', '<', 'v'];
    readonly record struct GuardState
    {
        public required Vec2 Position { get; init; }
        public required Vec2 Direction { get; init; }
        public Vec2 NextPos => Position + Direction;
    }

    // public int SolvePart1(string path)
    // {
    //     var world = File.ReadLines(path)
    //         .SelectMany((line, row) => line.Select((c, col) => (Pos: new Vec2(col, row), Char: c)))
    //         .ToFrozenDictionary(c => c.Pos, c => c.Char);
    //     
    //     var guardPos = world.First(f => GuardChars.Contains(f.Value))!.Key;
    //     var guard = new GuardState
    //     {
    //         Position = guardPos,
    //         Direction = GetDirection(world[guardPos])
    //     };
    //
    //     return GetGuardStates(guard, world).Select(g => g.Position).ToHashSet().Count;
    // }
    //
    // public int SolvePart2(string path)
    // {
    //     var world = File.ReadLines(path)
    //         .SelectMany((line, row) => line.Select((c, col) => (Pos: new Vec2(col, row), Char: c)))
    //         .ToFrozenDictionary(c => c.Pos, c => c.Char);
    //
    //     return world.Count(i => i.Value == '.' && WillCauseInfiniteLoop(i.Key, world.ToDictionary()));
    // }

    public int SolvePart1(Grid<short, char> grid)
    {
        var guardPos = grid.First(f => GuardChars.Contains(f.Value))!.Key;
        
        var guard = new GuardState
        {
            Position = guardPos,
            Direction = GetDirection(grid.Get(guardPos)!.Value)
        };
        
        return GetGuardStates(guard, grid).Select(g => g.Position).ToHashSet().Count;
    }

    public int SolvePart2(Grid<short, char> grid) => grid.Count(i =>
        i.Value == '.' && 
        WillCauseInfiniteLoop(grid.ReplaceAt(i.Key, '#')));

    public char MapCharToField(char c) => c;

    private static Vec2 GetDirection(char c) =>
        c switch
        {
            '^' => new Vec2(0, -1),
            '>' => new Vec2(1,  0),
            '<' => new Vec2(-1, 0),
            'v' => new Vec2(0,  1),
        };

    private static bool WillCauseInfiniteLoop(Grid<short, char> grid)
    {
        var allGuardStates = new HashSet<GuardState>();
        var guardPos = grid.First(f => GuardChars.Contains(f.Value))!.Key;
        var initialGuardState = new GuardState
        {
            Position = guardPos,
            Direction = GetDirection(grid.Get(guardPos)!.Value)
        };

        foreach (var guardState in GetGuardStates(initialGuardState, grid))
        {
            var guardWasAlreadyPresent = !allGuardStates.Add(guardState);
            if (guardWasAlreadyPresent)
            {
                return true;
            }
        }

        return false;
    }

    private static IEnumerable<GuardState> GetGuardStates(GuardState guardState, Grid<short, char> grid)
    {
        while (grid.ContainsPos(guardState.NextPos))
        {
            while (grid.Get(guardState.NextPos) == '#')
            {
                guardState = guardState with { Direction = guardState.Direction.RotateRight() };
                yield return guardState;
            }
            guardState = guardState with { Position = guardState.NextPos };
            yield return guardState;
        }
    }
}