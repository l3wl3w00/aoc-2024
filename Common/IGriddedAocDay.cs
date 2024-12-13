using System.Collections;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Numerics;

namespace Aoc._2024.Common;

public interface IGriddedAocDay<TResult, TVec, TField> : IAocDay<TResult> 
    where TResult : struct, IEquatable<TResult>
    where TVec : struct, INumber<TVec>
    where TField : struct
{
    
    TResult IAocDay<TResult>.SolvePart1(string path)
    {
        var grid = File.ReadLines(path)
            .SelectMany((l, row) => l.Select((c, col) => (Pos: new Vector2<TVec>(col, row), Char: MapCharToField(c))))
            .ToFrozenDictionary(c => c.Pos, c => c.Char);

        return SolvePart1(new Grid<TVec, TField>(grid));
    }

    TResult IAocDay<TResult>.SolvePart2(string path)
    {
        var grid = File.ReadLines(path)
            .SelectMany((l, row) => l.Select((c, col) => (Pos: new Vector2<TVec>(col, row), Char: MapCharToField(c))))
            .ToFrozenDictionary(c => c.Pos, c => c.Char);

        return SolvePart2(new Grid<TVec, TField>(grid));
    }

    TResult SolvePart1(Grid<TVec, TField> grid);
    TResult SolvePart2(Grid<TVec, TField> grid);
    TField MapCharToField(char c);
}

public interface IGriddedAocDay : IGriddedAocDay<int, int, int>;

public class Grid<TVec, TField> : IEnumerable<KeyValuePair<Vector2<TVec>, TField>>
    where TVec : struct, INumber<TVec>
    where TField : struct
{
    public Grid(IReadOnlyDictionary<Vector2<TVec>, TField> rawGrid)
    {
        RawGrid = rawGrid.ToImmutableDictionary();
    }

    public ImmutableDictionary<Vector2<TVec>, TField> RawGrid { get; }

    public TField? Get(Vector2<TVec> pos) => RawGrid.TryGetValue(pos, out var f) ? f : null;
    public IEnumerable<TField?> GetNonDiagonalNeighbours(Vector2<TVec> pos) => Vector2<TVec>
        .UpDownLeftRightDirs
        .Select(d => Get(pos + d));

    public IEnumerable<Vector2<TVec>> Positions => RawGrid.Keys;
    public IEnumerator<KeyValuePair<Vector2<TVec>, TField>> GetEnumerator() => RawGrid.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public bool ContainsPos(Vector2<TVec> pos)
    {
        return RawGrid.ContainsKey(pos);
    }

    public Grid<TVec, TField> ReplaceAt(Vector2<TVec> key, TField newValue)
    {
        return new Grid<TVec, TField>(RawGrid.SetItem(key, newValue));
    }
}