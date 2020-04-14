using Assets.Scripts.Dialogue.Variables.Attributes;

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
                        CharacterCreation.Instance.PoolPuzzleLoader.CompletePuzzle(Puzzle, false, true);

                    Puzzle = null;
                }
            }
        }

        public bool HasFoundAllEvidences => EvidencesFound == NumberOfEvidences;

        public string Puzzle { get; set; }

        public string AccusationState
        {
            get
            {
                if (EvidencesFound < NumberOfEvidences) return "SinPruebas";
                else return this == CharacterCreation.Instance.Murderer ? "Criminal" : "Inocente";
            }
        }

        [YarnAccess]
        public int RelationIndexCriminal;

        [YarnAccess]
        public int EmotionIndexCriminal;
    }
}
