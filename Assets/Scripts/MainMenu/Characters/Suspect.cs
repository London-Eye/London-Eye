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

        public string HasAlibiAsString => hasAlibi ? AffirmativeAlibi : NegativeAlibi;

        public int evidencesFound;

        public string Puzzle { get; set; }

        public string AccusationState
        {
            get
            {
                if (evidencesFound < numberOfEvidences) return "SinPruebas";
                else return this == CharacterCreation.Instance.Murderer ? "Criminal" : "Inocente";
            }
        }
    }
}
