using UnityEngine;

namespace Assets.Scripts.MainMenu.Characters
{
    [CreateAssetMenu(fileName = "CharacterStats", menuName = "CharacterStats")]
    public class CharacterStats : ScriptableObject
    {
        public string[] characterName;
        public string[] relation;
        public string[] emotion;
    }
}