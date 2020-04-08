using Assets.Scripts.Dialogue.Texts.Variables;

namespace Assets.Scripts.Characters
{
    [System.Serializable]
    public class Suspect : Character
    {
        public const string AffirmativeAlibi = "sí", NegativeAlibi = "no";
        public const int numberOfEvidences = 3;

        [YarnAccess]
        public string relation;

        [YarnAccess]
        public string emotion;

        [YarnAccess]
        public bool hasAlibi;

        private int evidencesFound;

        [YarnAccess]
        public string HasAlibiAsString => hasAlibi ? AffirmativeAlibi : NegativeAlibi;

        [YarnAccess]
        public string NotHasAlibiAsString => hasAlibi ? NegativeAlibi : AffirmativeAlibi;

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
