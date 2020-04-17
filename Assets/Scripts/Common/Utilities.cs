using Assets.Scripts.Dialogue.Tags;
using Assets.Scripts.Dialogue.Texts;
using Assets.Scripts.Dialogue.Variables.Storages;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn;
using Yarn.Unity;

namespace Assets.Scripts.Common
{
    public static class Utilities
    {
        public static Tag CreateColorTag(string color)
            => new Tag(new TagOption($"color={color}", TagFormat.RichTextTagFormat),
                new TagOption("color", TagFormat.RichTextTagFormat, TagOptionPosition.end));


        #region Yarn Value
        public static object As(this Value yarnValue, System.Type targetType)
        {
            if (targetType.IsAssignableFrom(typeof(float)))
            {
                return yarnValue.AsNumber;
            }
            else if (targetType.IsAssignableFrom(typeof(int)))
            {
                return (int)yarnValue.AsNumber;
            }
            else if (targetType.IsAssignableFrom(typeof(bool)))
            {
                return yarnValue.AsBool;
            }
            else if (targetType.IsAssignableFrom(typeof(string)))
            {
                return yarnValue.AsString;
            }
            else
            {
                return yarnValue;
            }
        }

        public static Value AsYarnValue(object obj)
        {
            Value value;

            if (obj is Value) value = obj as Value;
            else value = new Value(obj);

            return value;
        }
        #endregion


        #region Variable Storage Leading
        public const string variableLeading = "$";

        public static string AddLeadingIfNeeded(this VariableStorageBehaviour _, string text)
            => AddLeadingIfNeeded(text);

        public static string AddLeadingIfNeeded(string text)
            => (text == null || text.StartsWith(variableLeading)) ? text : (variableLeading + text);

        public static string RemoveLeadingIfPresent(this VariableStorageBehaviour _, string text)
            => RemoveLeadingIfPresent(text);

        public static string RemoveLeadingIfPresent(string text)
            => text.StartsWith(variableLeading) ? text.Remove(0, variableLeading.Length) : text;


        public static Value GetValueNoLeading(this VariableStorageBehaviour v, string variableNameNoLeading)
            => v.GetValue(AddLeadingIfNeeded(variableNameNoLeading));

        public static void SetValueNoLeading(this VariableStorageBehaviour v, string variableNameNoLeading, bool boolValue)
            => v.SetValue(AddLeadingIfNeeded(variableNameNoLeading), boolValue);
        public static void SetValueNoLeading(this VariableStorageBehaviour v, string variableNameNoLeading, float floatValue)
            => v.SetValue(AddLeadingIfNeeded(variableNameNoLeading), floatValue);
        public static void SetValueNoLeading(this VariableStorageBehaviour v, string variableNameNoLeading, string stringValue)
            => v.SetValue(AddLeadingIfNeeded(variableNameNoLeading), stringValue);
        public static void SetValueNoLeading(this VariableStorageBehaviour v, string variableNameNoLeading, Value value)
            => v.SetValue(AddLeadingIfNeeded(variableNameNoLeading), value);

        public static void SetValueNoLeading<T>(this AccessibleVariableStorage<T> v, string variableNameNoLeading, object objectValue) where T : VariableStorageBehaviour
            => v.SetValue(AddLeadingIfNeeded(variableNameNoLeading), objectValue);
        #endregion


        #region Post Game Dialogue
        public const string PostGameDialogueTag = "PostGame";

        public static string PostGameDialogueNode => $"{SceneManager.GetActiveScene().name}-{PostGameDialogueTag}";

        public static void StartPostGameDialogue()
            => StartPostGameDialogue(Object.FindObjectOfType<DialogueRunner>());

        public static void StartPostGameDialogue(DialogueRunner dialogueRunner)
            => dialogueRunner.StartDialogue(PostGameDialogueNode);
        #endregion


        #region Randomizers and other math utilities
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
        #endregion
    }
}
