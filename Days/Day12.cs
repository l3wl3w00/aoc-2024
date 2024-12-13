using System.Collections.Immutable;
using Aoc._2024.Common;

namespace Aoc._2024.Days;
using Vec2 = Vector2<int>;
public class Day12 : IGriddedAocDay<int, int, char>
{
    public int DayNumber { get; } = 12;
    public int ExpectedTestResultPart1 { get; } = 1930;
    public int? ExpectedTestResultPart2 { get; } = 1206;
    public int SolvePart1(Grid<int, char> grid)
    {
        HashSet<HashSet<Vec2>> groups = [];
        foreach (var pos in grid.Positions)
        {
            if (groups.Any(g => g.Contains(pos)))
            {
                continue;
            }
            groups.Add(GetGroup(grid, pos, []).ToHashSet());
        }
        
        return groups.Sum(group => 
            group.Count *
            group
                .SelectMany(p => p.NonDiagonalNeighbours.Where(n => grid.Get(n) != grid.Get(p)))
                .Count());
    }

    private IEnumerable<Vec2> GetGroup(Grid<int, char> grid, Vec2 pos, HashSet<Vec2> visited)
    {
        visited.Add(pos);
        return Vec2.UpDownLeftRightDirs
            .Where(d => grid.Get(pos + d) == grid.Get(pos) && !visited.Contains(pos + d))
            .SelectMany(d => GetGroup(grid, pos + d, visited))
            .Append(pos);
    }

    public int SolvePart2(Grid<int, char> grid)
    {
        HashSet<HashSet<Vec2>> groups = [];
        foreach (var field in grid)
        {
            if (groups.Any(g => g.Contains(field.Key)))
            {
                continue;
            }
            groups.Add(GetGroup(grid, field.Key, []).ToHashSet());
        }
        
        return groups.Sum(group =>
        {
            var groupNeighbours = group
                .SelectMany(p => p.NonDiagonalNeighbours.Where(n => grid.Get(n) != grid.Get(p)))
                .ToHashSet();

            var sidesSet = groupNeighbours
                    .Select(p => GetSide(groupNeighbours, p, []).ToImmutableHashSet())
                    .ToImmutableHashSet(new ImmutableHashSetComparer<Vec2>());
            Console.WriteLine(string.Join("\n",sidesSet.Select(s => string.Join(",", s))));
            var numberOfSides = sidesSet
                .Select(side => side.Select(sp => sp.NonDiagonalNeighbours.Count(group.Contains)).Min())
                .Sum();
            return group.Count * numberOfSides;
        });
        
    }

    public char MapCharToField(char c) => c;

    private IEnumerable<Vec2> GetSide(HashSet<Vec2> neighbours, Vec2 pos, HashSet<Vec2> visited)
    {
        visited.Add(pos);
        return Vec2.UpDownLeftRightDirs
            .Where(d => pos.DistanceTo(pos + d) < 1.00001 && 
                        !visited.Contains(pos + d) &&
                        neighbours.Contains(pos + d))
            .SelectMany(d => GetSide(neighbours, pos + d, visited))
            .Append(pos);
    }
}

public class ImmutableHashSetComparer<T> : IEqualityComparer<ImmutableHashSet<T>>
{
    public bool Equals(ImmutableHashSet<T> x, ImmutableHashSet<T> y)
    {
        // If both are null or both are reference-equal, consider them equal.
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;
        
        // Two sets are equal if they contain exactly the same elements.
        return x.SetEquals(y);
    }

    public int GetHashCode(ImmutableHashSet<T> obj)
    {
        if (obj is null) return 0;
        
        // A common technique is to combine the hash codes of all elements.
        // XOR is simple but effective for a basic hash combination.
        int hash = 0;
        foreach (var element in obj)
        {
            hash ^= EqualityComparer<T>.Default.GetHashCode(element);
        }
        return hash;
    }
}