using System.Collections.Frozen;
using System.Collections.Immutable;

namespace Aoc._2024.Days;

public class Day7 : IAocDay<long>
{
    public int DayNumber { get; } = 7;
    public long ExpectedTestResultPart1 { get; } = 3749;
    public long? ExpectedTestResultPart2 { get; } = 11387;

    enum OperationType
    {
        Multiply, Add, Concatenate
    }
    record Equation(IEnumerable<long> Numbers, List<OperationType> OperationTypes)
    {
        public long Calculate()
        {
            long result = Numbers.First();
            foreach (var n in Numbers.Skip(1).Enumerate())
            {
                result = OperationTypes[n.Idx] switch
                {
                    OperationType.Add => result + n.Value,
                    OperationType.Multiply => result * n.Value,
                    OperationType.Concatenate => long.Parse(result.ToString() + n.Value),
                };
            }

            return result;
        }
    }
    public long SolvePart1(string path)
    {
        return File.ReadLines(path)
            .Select(line => line.Split(": "))
            .Select(parts => (Result: long.Parse(parts[0]), Numbers: parts[1].Split().Select(long.Parse).ToList()))
            .Select(e => 
                GetAllSubsetsOf(e.Numbers.Select((_, idx) => (long)idx).ToFrozenSet())
                    .Select(perm => new Equation(
                        e.Numbers,
                        Enumerable
                            .Range(0, e.Numbers.Count)
                            .Select(n => perm.Contains(n) ? OperationType.Multiply : OperationType.Add)
                            .ToList()))
                    .FirstOrDefault(eq => eq.Calculate() == e.Result))
            .OfType<Equation>()
            .Sum(e => e.Calculate());
    }

    public long SolvePart2(string path)
    {
        return File.ReadLines(path)
            .Select(line => line.Split(": "))
            .Select(parts => (Result: long.Parse(parts[0]), Numbers: parts[1].Split().Select(long.Parse).ToList()))
            .Select(e =>
            {
                var numberIndices = e.Numbers.Select((_, idx) => (long)idx).ToImmutableHashSet();
                return GetAllSubsetsOf(numberIndices)
                    .Select(perm => (Perm: perm, Perms: GetAllSubsetsOf(numberIndices.Except(perm))))
                    .SelectMany(perm => perm.Perms.Select(p => new Equation(
                        e.Numbers,
                        Enumerable
                            .Range(0, e.Numbers.Count)
                            .Select(n => p.Contains(n) ? OperationType.Multiply : perm.Perm.Contains(n) ? OperationType.Concatenate : OperationType.Add)
                            .ToList())))
                    .FirstOrDefault(eq => eq.Calculate() == e.Result);
            })
            .OfType<Equation>()
            .Sum(e => e.Calculate());
    }

    private IEnumerable<FrozenSet<long>> GetAllSubsetsOf(ISet<long> longs)
    {
        var subsetCount = 1 << longs.Count; // 2^count
        for (var n = 0; n < subsetCount; n++)
        {
            var nCopy = n;
            yield return longs.Where((_, bit) => (nCopy & (1 << bit)) != 0).ToFrozenSet();
        }
    }
}