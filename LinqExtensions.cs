﻿namespace Aoc._2024;

public static class LinqExtensions
{
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var i in source)
        {
            action(i);
        }
    }
}