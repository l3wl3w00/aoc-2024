﻿namespace Aoc._2024.days;

public class Day3 : IAocDay<int>
{
    public int DayNumber { get; } = 3;
    public int ExpectedTestResultPart1 { get; } = 161;
    public int? ExpectedTestResultPart2 { get; } = 48;
    public int SolvePart1(string path) => CalculateMuls(string.Join("", File.ReadLines(path)));
    private int CalculateMuls(string memory) =>
        memory.Split("mul(")
            .Where(x => x.Contains(")"))
            .Select(i => i[..i.IndexOf(')')])
            .Select(i => i.Split(",").Select(TryParseIntoInt).ToList())
            .Where(ints => ints.Count == 2)
            .Sum(ints => ints[0] * ints[1]);
    private static int TryParseIntoInt(string input)
    {
        if (input.Length is 0 or > 3 || input.Any(c => !char.IsDigit(c)))
        {
            return 0;
        }

        int.TryParse(input, out var result);
        return result;
    }
    public int SolvePart2(string path) =>
        string.Join("", File.ReadLines(path))
            .Split("do()")
            .Select(i => new { Do = true, Value = i })
            .SelectMany(i => i.Value
                .Split("don't()")
                .Select((i2, idx) => new { Do = idx == 0, Value = i2 }))
            .Select(i => i.Do ? CalculateMuls(i.Value) : 0)
            .Sum();
}