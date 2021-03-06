using Assets.Scripts.Common;
using Assets.Scripts.Dialogue.Tags;
using Assets.Scripts.Dialogue.Texts;
using Assets.Scripts.Dialogue.Variables.Attributes;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    [System.Serializable]
    public class Suspect : Character
    {
        public const string AffirmativeAlibi = "sí", NegativeAlibi = "no";

        [YarnAccess]
        public const int NumberOfEvidences = 3;

        [YarnAccess]
        public string Relation;

        [YarnAccess]
        public string Emotion;

        [YarnAccess]
        public bool HasAlibi;

        private int evidencesFound;

        [YarnAccess]
        public string HasAlibiAsString => HasAlibi ? AffirmativeAlibi : NegativeAlibi;

        [YarnAccess]
        public string NotHasAlibiAsString => HasAlibi ? NegativeAlibi : AffirmativeAlibi;

        [YarnAccess]
        public int EvidencesFound
        {
            get => evidencesFound;
            set
            {
                evidencesFound = value;
                if (HasFoundAllEvidences)
                {
                    if (CharacterCreation.Instance != null)
                        CharacterCreation.Instance.PoolPuzzleLoader.CompletePuzzle(this);

                    Puzzle = null;
                }
            }
        }

        public bool HasFoundAllEvidences => EvidencesFound == NumberOfEvidences;

        public string Puzzle { get; set; }

        public enum AccusationState { SinPruebas, Criminal, Inocente }

        public AccusationState CurrentAccusationState
        {
            get
            {
                if (EvidencesFound < NumberOfEvidences-1) return AccusationState.SinPruebas;
                else return this == CharacterCreation.Instance.Murderer ? AccusationState.Criminal : AccusationState.Inocente;
            }
        }

        [YarnAccess]
        public int RelationIndexCriminal;

        [YarnAccess]
        public int EmotionIndexCriminal;

        public Sprite Image;

        [YarnAccess]
        public int Ending;
    }
}
