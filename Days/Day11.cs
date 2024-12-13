using Aoc._2024.Common;

namespace Aoc._2024.Days;

using NumberType = System.Int128;
public class Day11 : IAocDay<NumberType>
{
    public int DayNumber { get; } = 11;
    public NumberType ExpectedTestResultPart1 { get; } = 55312;
    public NumberType? ExpectedTestResultPart2 { get; } = NumberType.Parse("93457054879458202972686816015771");
    private static readonly NumberType[] PowersOf10 =
    [
        1L,
        10L,
        100L,
        1_000L,
        10_000L,
        100_000L,
        1_000_000L,
        10_000_000L,
        100_000_000L,
        1_000_000_000L,
        10_000_000_000L,
        100_000_000_000L,
        1_000_000_000_000L,
        10_000_000_000_000L,
        100_000_000_000_000L,
        1_000_000_000_000_000L,
        10_000_000_000_000_000L,
        100_000_000_000_000_000L,
        1_000_000_000_000_000_000L // max that fits in 64-bit long
    ];

    private NumberType Blink(NumberType currentStone, byte blinksLeft, Dictionary<(NumberType, byte), NumberType> cache)
    {
        if (blinksLeft == 0)
        {
            return 1;
        }

        if (cache.TryGetValue((currentStone, blinksLeft), out var cached))
        {
            return cached;
        }
    
        if (currentStone == 0)
        {
            var result = Blink(1, (byte)(blinksLeft - 1), cache);
            cache.TryAdd((currentStone, blinksLeft), result);
            return result;
        }
    
        var length = Length(currentStone);
        if (length % 2 == 0)
        {
            var divisor = PowersOf10[(byte)(length / 2)];
            var firstHalf = currentStone / divisor;
            var secondHalf = currentStone % divisor;
            var result = 
                Blink(firstHalf, (byte)(blinksLeft - 1), cache) + 
                Blink(secondHalf, (byte)(blinksLeft - 1), cache);
            cache.TryAdd((currentStone, blinksLeft), result);
            return result;
        }
        else
        {
            var result = Blink(currentStone * 2024, (byte)(blinksLeft - 1), cache);
            cache.TryAdd((currentStone, blinksLeft), result);
            return result;
        }
    }

    public NumberType SolvePart1(string path)
    {
        var stones = File.ReadAllText(path)
            .Split(' ')
            .Select(long.Parse)
            .ToList();
        NumberType sum = 0;
        foreach (var stone in stones)
        {
            sum += Blink(stone, 25, new Dictionary<(NumberType, byte), NumberType>());
        }
        return sum;
    }

    public NumberType SolvePart2(string path)
    {
        var stones = File.ReadAllText(path)
            .Split(' ')
            .Select(long.Parse)
            .ToList();
        NumberType sum = 0;
        foreach (var stone in stones)
        {
            sum += Blink(stone, 175, new Dictionary<(NumberType, byte), NumberType>());
        }
        return sum;
    }
    
    private static byte Length(NumberType n)
    {
        var low = 0;
        var high = PowersOf10.Length - 1;

        // Binary search to find the highest power of 10 <= n
        while (low < high)
        {
            var mid = (low + high + 1) >> 1;
            if (n >= PowersOf10[mid])
            {
                low = mid;
            }
            else
            {
                high = mid - 1;
            }
        }

        return (byte)(low + 0b1);
    }
}