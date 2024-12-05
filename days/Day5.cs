namespace Aoc._2024.days;

public class Day5 : IAocDay<int>
{
    public int DayNumber { get; } = 5;
    public int ExpectedTestResultPart1 { get; } = 143;
    public int? ExpectedTestResultPart2 { get; } = 123;

    private readonly record struct Rule(int Before, int After)
    {
        public bool Applies(List<int> updates)
        {
            if (!(updates.Contains(Before) && updates.Contains(After)))
            {
                return true;
            }

            var (before, after) = this;
            var beforeIndices = updates.Enumerate().Where(ui => ui.Value == before).ToList();
            var afterIndices = updates.Enumerate().Where(ui => ui.Value == after).ToList();
            return beforeIndices.TrueForAll(bi => afterIndices.TrueForAll(ai => bi.Idx < ai.Idx));
        }
        
        public void Apply(List<int> updates)
        {
            var (before, after) = this;
            var indices = updates
                .Enumerate()
                .Where(ui => ui.Value == before || ui.Value == after)
                .ToList();
            var beforeCount = indices.Count(i => i.Value == before);
            
            foreach (var i in indices.Select(i => i.Idx))
            {
                if (beforeCount > 0)
                {
                    beforeCount--;
                    updates[i] = Before;
                }
                else
                {
                    updates[i] = After;
                }
            }
        }
    }
    public int SolvePart1(string path)
    {
        var lines = File.ReadLines(path).ToList();
        var delimiterIdx = lines.FindIndex(l => l == string.Empty);
        var rules = lines[..delimiterIdx]
            .Select(l => l.Split('|').Select(int.Parse).ToList())
            .Select(ints => new Rule(ints[0], ints[1]))
            .ToList();

        return lines[(delimiterIdx + 1)..]
            .Select(l => l.Split(',').Select(int.Parse).ToList())
            .Where(u => rules.TrueForAll(r => r.Applies(u)))
            .Sum(updates => updates[updates.Count / 2]);
    }

    public int SolvePart2(string path)
    {
        var lines = File.ReadLines(path).ToList();
        var delimiterIdx = lines.FindIndex(l => l == string.Empty);
        var rules = lines[..delimiterIdx]
            .Select(l => l.Split('|').Select(int.Parse).ToList())
            .Select(ints => new Rule(ints[0], ints[1]))
            .ToList();
        
        return lines[(delimiterIdx + 1)..]
            .Select(l => l.Split(',').Select(int.Parse).ToList())
            .Where(u => !rules.TrueForAll(r => r.Applies(u)))
            .Select(updates => FixUpdates(rules, updates))
            .Sum(updates => updates[updates.Count / 2]);
    }

    private static List<int> FixUpdates(List<Rule> rules, List<int> updates)
    {
        while (rules.Exists(r => !r.Applies(updates)))
        {
            rules.ForEach(r => r.Apply(updates));
        }
        
        return updates;
    }
}