using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspect : MonoBehaviour
{
    [SerializeField] public CharacterStats maleCharacterStats;
    [SerializeField] public CharacterStats femaleCharacterStats;

    public bool isMale;
    public string characterName;
    public string relation;
    public string emotion;
    public bool hasAlibi;

    public void startSuspect(bool male, int nameIndex, int relationIndex, int emotionIndex, bool alibi)
    {
        isMale = male;
        name = char.ToString(maleCharacterStats.name[nameIndex]);
        Debug.Log("Suspect name: " + name);

        if (male)
        {
            
        } else
        {
            
        }
        hasAlibi = alibi;
    }
}
