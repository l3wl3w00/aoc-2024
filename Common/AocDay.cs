namespace Aoc._2024.Common;

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
        var inputPath = Path.Combine(RootPath, $"inputs/day{DayNumber}/input.txt");

        TResult testResult;
        TResult? expectedTestResult;
        
        if (partNumber == 1)
        {
            testResult = SolvePart1(testInputPath);
            expectedTestResult = ExpectedTestResultPart1;
        }
        else
        {
            testResult = SolvePart2(testInputPath);
            expectedTestResult = ExpectedTestResultPart2;
        }
        
        Console.WriteLine($"Part {partNumber} test result: {testResult}");
        if (!testResult.Equals(expectedTestResult))
        {
            Console.Error.WriteLine($"Part {partNumber} test result is incorrect!");
            return;
        }
        
        var result = partNumber == 1 ? SolvePart1(inputPath) : SolvePart2(inputPath);
        Console.WriteLine($"Part {partNumber} result: {result}");
    }

    TResult SolvePart1(string path);
    TResult SolvePart2(string path);
}