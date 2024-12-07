using System.Collections.Frozen;
using System.Collections.Immutable;

namespace Aoc._2024.Days;
using Vec2 = Vector2<short>;

public class Day6 : IAocDay<int>
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

    public int SolvePart1(string path)
    {
        var world = File.ReadLines(path)
            .SelectMany((line, row) => line.Select((c, col) => (Pos: new Vec2(col, row), Char: c)))
            .ToFrozenDictionary(c => c.Pos, c => c.Char);
        
        var guardPos = world.First(f => GuardChars.Contains(f.Value))!.Key;
        var guard = new GuardState
        {
            Position = guardPos,
            Direction = GetDirection(world[guardPos])
        };
        
        return GetGuardStates(guard, world).Select(g => g.Position).ToHashSet().Count;
    }

    public int SolvePart2(string path)
    {
        var world = File.ReadLines(path)
            .SelectMany((line, row) => line.Select((c, col) => (Pos: new Vec2(col, row), Char: c)))
            .ToFrozenDictionary(c => c.Pos, c => c.Char);

        return world.Count(i => i.Value == '.' && WillCauseInfiniteLoop(i.Key, world.ToDictionary()));
    }

    private static Vec2 GetDirection(char c) =>
        c switch
        {
            '^' => new Vec2(0, -1),
            '>' => new Vec2(1,  0),
            '<' => new Vec2(-1, 0),
            'v' => new Vec2(0,  1),
        };

    private static bool WillCauseInfiniteLoop(Vec2 obstructionPos, IDictionary<Vec2, char> world)
    {
        world[obstructionPos] = '#';

        var allGuardStates = new HashSet<GuardState>();
        var guardPos = world.First(f => GuardChars.Contains(f.Value))!.Key;
        var initialGuardState = new GuardState
        {
            Position = guardPos,
            Direction = GetDirection(world[guardPos])
        };

        foreach (var guardState in GetGuardStates(initialGuardState, world))
        {
            var guardWasAlreadyPresent = !allGuardStates.Add(guardState);
            if (guardWasAlreadyPresent)
            {
                return true;
            }
        }

        return false;
    }

    private static IEnumerable<GuardState> GetGuardStates(GuardState guardState, IDictionary<Vec2, char> world)
    {
        while (world.ContainsKey(guardState.NextPos))
        {
            while (world.TryGetValue(guardState.NextPos, out var c) && c == '#')
            {
                guardState = guardState with { Direction = guardState.Direction.RotateRight() };
                yield return guardState;
            }
            guardState = guardState with { Position = guardState.NextPos };
            yield return guardState;
        }
    }
}