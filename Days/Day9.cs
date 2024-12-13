using Aoc._2024.Common;

namespace Aoc._2024.Days;

public class Day9 : IAocDay<long>
{
    const int EMPTY_CELL_VALUE = -1;
    public int DayNumber { get; } = 9;
    public long ExpectedTestResultPart1 { get; } = 1928;
    public long? ExpectedTestResultPart2 { get; } = 2858;
    
    public long SolvePart1(string path)
    {
        var block = File.ReadAllText(path).SelectMany((c, idx) =>
        {
            var isFile = idx % 2 == 0;
            var value = int.Parse(c.ToString());
            if (isFile)
            {
                var fileId = idx / 2;
                return Enumerable.Repeat((long)fileId, value);
            }

            return Enumerable.Repeat((long)EMPTY_CELL_VALUE, value);
        }).ToList();
        var files = block.Enumerate().Where(c => c.Value != EMPTY_CELL_VALUE).Reverse().ToList();

        var emptyIdx = 0;
        
        for (int i = 0; i < files[emptyIdx].Idx; i++)
        {
            if (block[i] == EMPTY_CELL_VALUE)
            {
                block[i] = files[emptyIdx].Value;
                block[files[emptyIdx].Idx] = EMPTY_CELL_VALUE;
                emptyIdx++;
            }
        }

        return block.Enumerate().Where(i => i.Value != EMPTY_CELL_VALUE).Sum(i => i.Idx * i.Value);
    }

    public long SolvePart2(string path)
    {
        var block = File.ReadAllText(path).SelectMany((c, idx) =>
        {
            var isFile = idx % 2 == 0;
            var value = int.Parse(c.ToString());
            if (isFile)
            {
                var fileId = idx / 2;
                return Enumerable.Repeat((long)fileId, value);
            }

            return Enumerable.Repeat((long)EMPTY_CELL_VALUE, value);
        }).ToList();
        List<(long FileId, int StartIdx, int Count)> files = block.Enumerate()
            .Where(c => c.Value != EMPTY_CELL_VALUE)
            .GroupBy(x => x.Value)
            .Select(x => (x.Key, x.First().Idx, x.Count()))
            .Reverse()
            .ToList();

        List<(int StartIdx, int Count)> emptyBlocks = [];
        var currentBlockSize = 0;
        foreach (var b in block.Enumerate().Where(c => c.Value == EMPTY_CELL_VALUE))
        {
            currentBlockSize++;
            if (block[b.Idx + 1] != EMPTY_CELL_VALUE)
            {
                emptyBlocks.Add((b.Idx - currentBlockSize + 1, currentBlockSize));
                currentBlockSize = 0;
            }

        }
        foreach (var fileBlock in files)
        {
            var emptyBlockIdx = emptyBlocks.FindIndex(eb => eb.Count >= fileBlock.Count);
            if (emptyBlockIdx == -1)
            {
                continue;
            }
            var emptyBlock = emptyBlocks[emptyBlockIdx];
            if (fileBlock.StartIdx < emptyBlock.StartIdx + fileBlock.Count)
            {
                break;
            }
            for (var i = emptyBlock.StartIdx; i < emptyBlock.StartIdx + fileBlock.Count; i++)
            {
                block[i] = fileBlock.FileId;
                block[fileBlock.StartIdx + i - emptyBlock.StartIdx] = EMPTY_CELL_VALUE;
            }
            var newCount = emptyBlock.Count - fileBlock.Count;
            if (newCount <= 0)
            {
                emptyBlocks.RemoveAt(emptyBlockIdx);
            }
            else
            {
                emptyBlocks[emptyBlockIdx] = emptyBlock with { StartIdx = emptyBlock.StartIdx + fileBlock.Count,Count = newCount };
            }
        }
        
        return block.Enumerate().Where(i => i.Value != EMPTY_CELL_VALUE).Sum(i => i.Idx * i.Value);
        // 6416577179091 too high
    }
}