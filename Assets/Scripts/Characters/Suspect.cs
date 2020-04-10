using Assets.Scripts.Dialogue.Variables.Attributes;

namespace Assets.Scripts.Characters
{
    [System.Serializable]
    public class Suspect : Character
    {
        public const string AffirmativeAlibi = "sí", NegativeAlibi = "no";
        public const int numberOfEvidences = 3;

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
                    PoolPuzzleLoader.CompletePuzzle(Puzzle, false);
                    Puzzle = null;
                }
            }
        }

        public bool HasFoundAllEvidences => EvidencesFound == numberOfEvidences;

        public string Puzzle { get; set; }

        public string AccusationState
        {
            get
            {
                if (EvidencesFound < numberOfEvidences) return "SinPruebas";
                else return this == CharacterCreation.Instance.Murderer ? "Criminal" : "Inocente";
            }
        }
    }
}
