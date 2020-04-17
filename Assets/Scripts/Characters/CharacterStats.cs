using UnityEngine;

namespace Assets.Scripts.Characters
{
    [CreateAssetMenu(fileName = "CharacterStats", menuName = "CharacterStats")]
    public class CharacterStats : ScriptableObject
    {
        public string[] characterName;
        public string[] relation;
        public string[] emotion;
        public Sprite[] images;
    }
}