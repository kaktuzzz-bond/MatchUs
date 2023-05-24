using System;
using System.Collections.Generic;
using System.Linq;

public static class ListExtensions
{
    public static List<T> Shuffle<T>(this IEnumerable<T> source)
    {
        Random r = new ();

        return source
                .OrderBy(_ => r.Next())
                .ToList();
    }
}