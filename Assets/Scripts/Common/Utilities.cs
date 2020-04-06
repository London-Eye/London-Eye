using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

namespace Assets.Scripts.Common
{
    public static class Utilities
    {
        public const string PostGameDialogueTag = "PostGame";

        public static string PostGameDialogueNode => $"{SceneManager.GetActiveScene().name}-{PostGameDialogueTag}";

        public static void StartPostGameDialogue()
            => StartPostGameDialogue(Object.FindObjectOfType<DialogueRunner>());

        public static void StartPostGameDialogue(DialogueRunner dialogueRunner)
            => dialogueRunner.StartDialogue(PostGameDialogueNode);


        public static T[] GetShuffle<T>(this IEnumerable<T> arr)
        {
            T[] newArray = arr.ToArray();
            newArray.Shuffle();
            return newArray;
        }

        /// <summary>
        /// Knuth shuffle algorithm :: courtesy of Wikipedia :)
        /// </summary>
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

        public static bool RandomBool() => Random.value > 0.5f;

        /// <summary>
        /// Add to <paramref name="collection"/> the numbers from <paramref name="start"/> [inclusive] to <paramref name="end"/> [exclusive].
        /// </summary>
        /// <param name="start">Start of the range [inclusive]</param>
        /// <param name="end">End of the range [exclusive]</param>
        /// <returns>The same collection from <paramref name="collection"/></returns>
        public static T AddIntRange<T>(T collection, int start, int end) where T : ICollection<int>
        {
            for (int i = start; i < end; i++)
            {
                collection.Add(i);
            }
            return collection;
        }
    }
}
