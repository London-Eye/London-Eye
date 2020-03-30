using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspect : MonoBehaviour
{
    [SerializeField] public CharacterStats characterStats;

    public bool isMale;
    public int nameIndex;
    public int relationIndex;
    public int emotionIndex;
    public bool hasAlibi;

    public void startSuspect(bool male, int name, int relation, int emotion, bool alibi)
    {
        isMale = male;
        nameIndex = name;
        relationIndex = relation;
        emotionIndex = emotion;
        hasAlibi = alibi;
        if(male)
        {
            characterStats = Resources.Load<CharacterStats>("MaleSuspect");
        } else
        {
            characterStats = Resources.Load<CharacterStats>("FemaleSuspect");
        }
    }
}
