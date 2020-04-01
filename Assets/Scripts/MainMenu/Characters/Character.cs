using UnityEngine;

[System.Serializable]
public class Character : ScriptableObject
{
    public const string AffirmativeAlibi = "sí", NegativeAlibi = "no";

    public bool isMale;
    public string cname;
    public string relation;
    public string emotion;
    public bool hasAlibi;

    public string HasAlibiAsString => hasAlibi ? AffirmativeAlibi : NegativeAlibi;
}
