namespace Aoc._2024;

public static class LinqExtensions
{
    public static IEnumerable<T> Peek<T>(this IEnumerable<T> source, Action<T> action)
    {
        return source.Select(x =>
        {
            action(x);
            return x;
        });
    }
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var i in source)
        {
            action(i);
        }
    }
    
    public static IEnumerable<(int Idx, T Value)> Enumerate<T>(this IEnumerable<T> source)
    {
        return source.Select((x, i) => (Idx: i, Value: x));
    }
}