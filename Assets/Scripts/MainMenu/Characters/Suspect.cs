namespace Assets.Scripts.MainMenu.Characters
{
    [System.Serializable]
    public class Suspect : Character
    {
        public const string AffirmativeAlibi = "sí", NegativeAlibi = "no";

        public string relation;
        public string emotion;
        public bool hasAlibi;

        public string HasAlibiAsString => hasAlibi ? AffirmativeAlibi : NegativeAlibi;

        public string Puzzle { get; set; }
    }
}
