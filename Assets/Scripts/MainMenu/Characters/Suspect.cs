namespace Assets.Scripts.MainMenu.Characters
{
    [System.Serializable]
    public class Suspect : Character
    {
        public const string AffirmativeAlibi = "sí", NegativeAlibi = "no";
        public const int numberOfEvidences = 3;

        public string relation;
        public string emotion;
        public bool hasAlibi;
        private int evidencesFound;

        public string HasAlibiAsString => hasAlibi ? AffirmativeAlibi : NegativeAlibi;
        public string NotHasAlibiAsString => hasAlibi ? NegativeAlibi : AffirmativeAlibi;

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
