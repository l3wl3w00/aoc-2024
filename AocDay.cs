﻿namespace Aoc._2024;

public interface IAocDay
{
    void Solve();
    int DayNumber { get; }
}
public interface IAocDay<TResult> : IAocDay
    where TResult : struct, IEquatable<TResult> 
{
    private const string RootPath =
        "C:\\Main\\OwnProgrammingProjects\\AdventOfCode\\Aoc.2024";

    TResult ExpectedTestResultPart1 { get; }
    TResult? ExpectedTestResultPart2 { get; }

    void IAocDay.Solve()
    {
        Solve(partNumber: 1);
        var solvePart2 = ExpectedTestResultPart2 is not null;
        if (solvePart2)
        {
            Solve(partNumber: 2); 
        }
    }
    
    void Solve(int partNumber)
    {
        var testInputPath = Path.Combine(RootPath, $"inputs/day{DayNumber}/part{partNumber}.testinput.txt");
        var inputPath = Path.Combine(RootPath, $"inputs/day{DayNumber}/part{partNumber}.input.txt");

        TResult testResult;
        TResult result;
        TResult? expectedTestResult;
        
        if (partNumber == 1)
        {
            testResult = SolvePart1(testInputPath);
            result = SolvePart1(inputPath);
            expectedTestResult = ExpectedTestResultPart1;
        }
        else
        {
            testResult = SolvePart2(testInputPath);
            result = SolvePart2(inputPath);
            expectedTestResult = ExpectedTestResultPart2;
        }
        
        Console.WriteLine($"Part {partNumber} test result: {testResult}");
        if (!testResult.Equals(expectedTestResult))
        {
            Console.Error.WriteLine($"Part {partNumber} test result is incorrect!");
            return;
        }
        Console.WriteLine($"Part {partNumber} result: {result}");
    }

    TResult SolvePart1(string path);
    TResult SolvePart2(string path);
}