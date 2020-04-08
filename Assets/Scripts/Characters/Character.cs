using Assets.Scripts.Dialogue.Texts.Variables;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    [System.Serializable]
    public class Character : ScriptableObject
    {
        [YarnAccess]
        public bool isMale;

        [YarnAccess]
        public string cname;
    }
}