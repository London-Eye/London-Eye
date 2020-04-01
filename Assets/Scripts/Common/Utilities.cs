using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public static class Utilities
    {   
        public static T[] GetShuffle<T>(this IEnumerable<T> arr)
        {
            T[] newArray = arr.ToArray();
            newArray.Shuffle();
            return newArray;
        }

        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        public static void Shuffle<T>(this T[] arr)
        {
            for (int t = 0; t < arr.Length - 1; t++)
            {
                T tmp = arr[t];
                int r = Random.Range(t, arr.Length);
                arr[t] = arr[r];
                arr[r] = tmp;
            }
        }
    }
}
