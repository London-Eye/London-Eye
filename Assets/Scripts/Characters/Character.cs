using Assets.Scripts.Dialogue.Texts.Variables.Attributes;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    [System.Serializable]
    public class Character : ScriptableObject
    {
        [YarnAccess]
        public bool IsMale;

        [YarnAccess(name = "Name")]
        public string cname;
    }
}