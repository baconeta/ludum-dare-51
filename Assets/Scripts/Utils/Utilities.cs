using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class Utilities
    {
        public static T ChooseRandom<T>(this IReadOnlyList<T> options)
            => options[Random.Range(0, options.Count)];
    }
}