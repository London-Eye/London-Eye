using Assets.Scripts.Dialogue.Variables.Attributes;

namespace Assets.Scripts.Characters
{
    [System.Serializable]
    public class Victim : Character
    {
        [YarnAccess]
        public bool Investigated;
    }
}
