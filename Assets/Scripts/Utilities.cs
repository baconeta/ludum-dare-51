using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static T ChooseRandom<T>(this IReadOnlyList<T> options)
        => options[Random.Range(0, options.Count)];
}
